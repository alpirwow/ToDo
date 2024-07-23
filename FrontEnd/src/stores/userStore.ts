import {defineStore} from "pinia";
import {type UserGetDto} from "@/dto/user/UserGetDto"

export const useMyUserStore = defineStore({
    id: 'user',
    state: () => ({
        user: null as UserGetDto | null,
    }),
    actions: {
        setUser(value: UserGetDto) {
            this.user = value;
        },
        getUser(): UserGetDto | null {
            return this.user;
        },
        clearUser() {
            this.user = null;
        }
    },
});