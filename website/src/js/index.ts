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

let ShowTasksB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("showTasks");
ShowTasksB.addEventListener("click", ShowTasks)

function AddTask(): void {
    let title: string = (<HTMLInputElement>document.getElementById("taskTitle")).value;
    axios.post<Task>(taskUri, {uid: 1, description: title})
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
}

function ShowTasks(): void {
    axios.get<Task[]>(taskUri)
        .then(function (response: AxiosResponse<Task[]>): void {
            let result: string = "<table><tr>"+
            "<th>Tid</th>"+
            "<th>Uid</th>"+
            "<th>Timestamp</th>"+
            "<th>Endstamp</th>"+
            "<th>Description</th>"+
            "<th>Done</th>"+
            "</tr>";

            response.data.forEach((task: Task) => {
                result += "<tr><td>" + task.tid + "</td>"
                + "<td>" + task.uid + "</td>"
                + "<td>" + task.timestamp + "</td>"
                + "<td>" + task.endstamp + "</td>"
                + "<td>" + task.description + "</td>"
                + "<td>" + task.done + "</td></tr>";
            })
            result += "</table>";

            Return.innerHTML = result;
        })
        .catch(function (error: AxiosError): void {
            Return.innerHTML = ""+error;
        })
      console.log("Done");
}