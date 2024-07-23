import {defineStore} from "pinia";

export const useLoadAnimationStore = defineStore({
    id: 'loadAnimation',
    state: () => ({
        isLoading: false,
    }),
    actions: {
        setLoading(value: boolean) {
            this.isLoading = value;
        },
    },
});