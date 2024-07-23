import type {ToDoItemGetDto} from "@/dto/toDoItem/ToDoItemGetDto";

export interface UserGetDto {
    id: string;
    name: string;
    toDoItems?: ToDoItemGetDto[] | null;
}
