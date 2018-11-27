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

var uri: string = "https://berit.azurewebsites.net/api/";

let Return: HTMLDivElement = <HTMLDivElement>document.getElementById("return");

let AddTaskB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("taskAdd");
AddTaskB.addEventListener("click", AddTask);

function AddTask(): void {
    let title: string = (<HTMLInputElement>document.getElementById("taskTitle")).value;
    let tempUri: string = uri + ""
    axios.post<Task>(tempUri, {uid: 1, description: title})
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
}

function deleteCar(): void {
    let output: HTMLDivElement = <HTMLDivElement>document.getElementById("contentDelete");
    let inputElement: HTMLInputElement = <HTMLInputElement>document.getElementById("deleteInput");
    let tempUri: string = uri + inputElement.value;
    axios.delete<Task>(tempUri)
        .then(function (response: AxiosResponse<Task>): void { Return.innerHTML = "response " + response.status + " " + response.statusText;})
        .catch(function (error: AxiosError): void {Return.innerHTML = ""+error;});
}
