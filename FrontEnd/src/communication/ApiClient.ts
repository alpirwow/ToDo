import axios, {type AxiosRequestConfig, type AxiosResponse} from 'axios';
import {useMyUserStore} from "@/stores/userStore";
import {useLoadAnimationStore} from "@/stores/loadAnimationStore";

export class ApiClient {
    private readonly axiosInstance = axios.create({
        baseURL: import.meta.env.VITE_APP_API_URL,
        withCredentials: true,
        timeout: 10000,
        headers: {
            common: {
                'Content-Type': 'application/json; charset=utf-8'
            }
        }
    });

    private activeRequests = 0;

    constructor() {
        this.axiosInstance.interceptors.request.use((config) => {
            const userId = useMyUserStore().getUser()?.id;

            if (userId)
                config.headers['ToDo-User-Id'] = userId;

            return config;
        });
    }

    private startRequest() {
        this.activeRequests++;
        const store = useLoadAnimationStore();
        store.setLoading(true);
    }

    private endRequest() {
        this.activeRequests--;
        if (this.activeRequests === 0) {
            const store = useLoadAnimationStore();
            store.setLoading(false);
        }
    }

    public async get<T = any>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        this.startRequest();

        try {
            return await this.axiosInstance.get<T>(url, config);
        } catch (error) {
            console.error(error);
            throw error;
        } finally {
            this.endRequest();
        }
    }

    public async getWithParams<T = any>(url: string, params: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        this.startRequest();

        try {
            return await this.axiosInstance.get<T>(url, {...config, params});
        } catch (error) {
            console.error(error);
            throw error;
        } finally {
            this.endRequest();
        }
    }

    public async post<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        this.startRequest();

        try {
            return await this.axiosInstance.post<T>(url, data, config);
        } catch (error) {
            console.error(error);
            throw error;
        } finally {
            this.endRequest();
        }
    }

    public async put<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        this.startRequest();

        try {
            return await this.axiosInstance.put<T>(url, data, config);
        } catch (error) {
            console.error(error);
            throw error;
        } finally {
            this.endRequest();
        }
    }

    public async delete<T = any>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        this.startRequest();

        try {
            return await this.axiosInstance.delete<T>(url, config);
        } catch
            (error) {
            console.error(error);
            throw error;
        } finally {
            this.endRequest();
        }
    }
}

export const apiClient = new ApiClient();