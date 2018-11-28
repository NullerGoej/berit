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

var loading: string = "<h1>Loading...</h1>";

var uri: string = "https://berit.azurewebsites.net/api/";

let Return: HTMLDivElement = <HTMLDivElement>document.getElementById("return");

let AddTaskB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("taskAdd");
AddTaskB.addEventListener("click", AddTask);

let ShowTasksB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("showTasks");
ShowTasksB.addEventListener("click", ShowTasks)

let ShowCompletedTasksB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("showCompletedTasks");
ShowCompletedTasksB.addEventListener("click", ShowCompletedTasks);

let DeleteTaskInp: HTMLInputElement = <HTMLInputElement>document.getElementById("taskIdInp");
let DeleteTaskB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("deleteTask");
DeleteTaskB.addEventListener("click", function() { DeleteTask(DeleteTaskInp.value); });

function AddTask(): void {
    let title: string = (<HTMLInputElement>document.getElementById("taskTitle")).value;
    axios.post<Task>(uri + "task", {uid: 1, description: title})
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
}

// Vagner
// Showing all tasks, not filtered
function ShowTasks(): void {
    Return.innerHTML = loading;
    axios.get<Task[]>(uri + "task")
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
                + "<td>" + task.done + "</td></tr>"
            })
            result += "</table>";

            Return.innerHTML = result;
        })
        .catch(function (error: AxiosError): void {
            Return.innerHTML = ""+error;
        })
}

// Vagner 
// Showing tasks that has been completed
function ShowCompletedTasks(): void {
    Return.innerHTML = loading;
    axios.get<Task[]>(uri + "task")
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
            if (task.done)
            {
                result += "<tr><td>" + task.tid + "</td>"
                + "<td>" + task.uid + "</td>"
                + "<td>" + task.timestamp + "</td>"
                + "<td>" + task.endstamp + "</td>"
                + "<td>" + task.description + "</td>"
                + "<td>" + task.done + "</td></tr>";
            }            
        })
        result += "</table>";

        Return.innerHTML = result;
    })
    .catch(function (error: AxiosError): void {
        Return.innerHTML = ""+error;
    })
}

// Vagner
// Deletes a task completely from the database
function DeleteTask(tid: string): void {
    axios.delete(uri + "task/" + tid)
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
}
