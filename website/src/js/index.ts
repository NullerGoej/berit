import axios, {
    AxiosResponse,
    AxiosError} 
    from "../../node_modules/axios/index";

interface Task {
    tid: number;
    uid: number;
    timestamp: Date;
    endstamp: Date;
    description: string;
    done: boolean;
}

var taskUri: string = "https://berit.azurewebsites.net/api/task/";

let Return: HTMLDivElement = <HTMLDivElement>document.getElementById("return");

let AddTaskB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("taskAdd");
AddTaskB.addEventListener("click", AddTask);

function AddTask(): void {
    let title: string = (<HTMLInputElement>document.getElementById("taskTitle")).value;
    axios.post<Task>(taskUri, {uid: 1, description: title})
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
}