import axios, {AxiosResponse,AxiosError} from "../../node_modules/axios/index";

//Bastian og Vagner
interface Task {
    tid: number;
    uid: number;
    timestamp: Date;
    endstamp: Date;
    description: string;
    done: boolean;
}

//Bastian
interface PiData {
    pid: number;
    timestamp: Date;
    temperatur: number;
}

//Sander og Casper
interface ReTask {
    trid: number;
    uid: number;
    createdate: Date;
    completedate: Date;
    description: string;
    count : number;
    done: boolean;
}

interface Alarm {
    aid: number;
    uid: number;
    timestamp: Date;
    asid: number;
}

var loading: string = "<h2>Loading...</h2>";

var uri: string = "https://berit.azurewebsites.net/api/";

let Return: HTMLDivElement = <HTMLDivElement>document.getElementById("return");

let TemperatureDiv: HTMLDivElement = <HTMLDivElement>document.getElementById("temperature");

let AddTaskB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("taskAdd");
AddTaskB.addEventListener("click", AddTask);

let ShowTasksB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("showTasks");
ShowTasksB.addEventListener("click", ShowTasks)

let ShowCompletedTasksB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("showCompletedTasks");
ShowCompletedTasksB.addEventListener("click", ShowCompletedTasks);

let ShowReTaskBtnF: HTMLButtonElement = <HTMLButtonElement>document.getElementById("showReTaskBtn");
ShowReTaskBtnF.addEventListener("click", ShowReTask)

let DeleteTaskInp: HTMLInputElement = <HTMLInputElement>document.getElementById("taskIdInp");
let DeleteTaskB: HTMLButtonElement = <HTMLButtonElement>document.getElementById("deleteTask");
DeleteTaskB.addEventListener("click", function() { DeleteTask(DeleteTaskInp.value); });

//Bastian & Casper
//Google News Api Treatment
let Newscard: HTMLDivElement = <HTMLDivElement>document.getElementById("NewsCard");
let NewsRequset = new XMLHttpRequest();

NewsRequset.open('GET','https://newsapi.org/v2/top-headlines?sources=google-news&apiKey=30ebfbe1934d4229904d2f40e07ad194',true)

NewsRequset.onload = function () {
var NewsDeck = JSON.parse(this.response)
console.log(NewsDeck);
let divInput : string = "<div style='display: flex;flex-wrap: wrap;text-align:center;left:20%;position:relative;width: 60%'>";
let index : number = 0;
NewsDeck.articles.forEach(element => {
if(index == 4){
// nothing :D
} else {
divInput += "<div style='flex-basis: calc(50% - 20px);margin: 10px;'><a href='"+element.url+"' target='_blank'><img src='"+element.urlToImage+"' width='100%'></a><h3>"+element.title+"</h3><p>"+element.description+"</p></div>";
index++;}
});
Newscard.innerHTML = divInput+"</div>";
}
NewsRequset.send();

let NewAlarmDateInp: HTMLInputElement = <HTMLInputElement>document.getElementById("newAlarmDateInp");
let NewAlarmToneInp: HTMLInputElement = <HTMLInputElement>document.getElementById("newAlarmToneInp");
let NewAlarmBtn: HTMLButtonElement = <HTMLButtonElement>document.getElementById("newAlarmBtn");
NewAlarmBtn.addEventListener("click", CreateAlarm);

ShowNewestTemperature();

// Functions

// Bastian
// Showing newest tempature
function ShowNewestTemperature(): void {
    TemperatureDiv.innerHTML = "Loading...";
    axios.get<PiData>(uri + "pidata/GetNewest")
    .then(function (response: AxiosResponse<PiData>): void {
        TemperatureDiv.innerHTML = response.data.temperatur+"°C";
    })
    .catch(function (error: AxiosError): void {
        TemperatureDiv.innerHTML = ""+error;
    })
}

// Bastian
// Adding a task
function AddTask(): void {
    let title: string = (<HTMLInputElement>document.getElementById("taskTitle")).value;
    let taskDate: Date = (<HTMLInputElement>document.getElementById("taskDate")).value;
    let count: number = (<HTMLInputElement>document.getElementById("count")).valueAsNumber;

    if (count == 0)
    axios.post<Task>(uri + "task", {uid: 1, description: title, })
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
    if (count > 0)
    console.log(count)
    axios.post<ReTask>(uri + "taskRepeat", {uid: 1, description: title, count: count, completedate: taskDate})
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
}

// Vagner
// Showing all tasks, not filtered
function ShowTasks(): void {
    Return.innerHTML = loading;
    axios.get<Task[]>(uri + "task")
        .then(function (response: AxiosResponse<Task[]>): void {
            let result: HTMLDivElement = <HTMLDivElement>document.createElement("DIV");
            let res = result.innerHTML;
            res += "<table id='tasksTbl'><tr>"+
            "<th>Id</th>"+
            "<th>Description</th>"+
            "<th>Check</th>"+
            "</tr>";
            Return.innerHTML = result.innerHTML;


            response.data.forEach((task: Task) => {
                if (!task.done)
                {
                    res += "<tr id='task" + task.tid + "'><td>" + task.tid + "</td>"
                    + "<td>" + task.description + "</td>"
                    + "<td><button id='" + task.tid + "' class='completeBtn' style='position: relative; left: 40%'>Complete</button></td></tr>";
                }
            })
            res += "</table>";

            Return.innerHTML = res;

            var btns = document.getElementsByClassName('completeBtn');
            for (var i = 0; i < btns.length; i++)
            {
                (function() {
                    var btn = btns[i];
                    var id = btn.id;
                    btn.addEventListener("click", function() { CompleteTask(id); });
                }());
            }

            //Return.innerHTML = res;
        })
        .catch(function (error: AxiosError): void {
            Return.innerHTML = "" + error;
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


// Vagner
// Updates a task to be completed at current timestamp, then shows the Completed Tasks table
function CompleteTask(taskid: string): void
{
    axios.get<Task>(uri + "task/" + taskid)
        .then(function (response: AxiosResponse<Task>): void {
            let task = response.data;
            axios.put(uri + 'task/' + taskid, {description:task.description, done:true})
                .then((response) => { Return.innerHTML = "response " + response.status + " " + response.statusText; ShowCompletedTasks(); })
                .catch(function(error) {console.log(error); })
        })
        .catch(function (error: AxiosError): void {
            Return.innerHTML = ""+error;
        })
}


// Vagner
// Create new Alarm
function CreateAlarm()
{
    let date = NewAlarmDateInp.value;
    let tone = NewAlarmToneInp.value;
    axios.post<Alarm>(uri + "alarm", {uid: 1, timestamp: date, asid: tone})
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
}

function CompleteReTask(retaskid: string): void
{
    axios.get<ReTask>(uri + "taskrepeat/" + retaskid)
        .then(function (response: AxiosResponse<ReTask>): void {
            let retask = response.data;  
            let tempdate = new Date(retask.completedate.toString())
            tempdate.setDate(tempdate.getDate()+retask.count);
            console.log(tempdate)

             axios.post<ReTask>(uri + "taskRepeat", {uid: 1, description: retask.description, count: retask.count, completedate: tempdate})
            .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
            .catch((error: AxiosError) => { Return.innerHTML = ""+error; });

        })
    DeleteReTask(retaskid);
}
function ShowReTask(): void {
    Return.innerHTML = loading;
    axios.get<ReTask[]>(uri + "taskrepeat")
        .then(function (response: AxiosResponse<ReTask[]>): void {
            let result: HTMLDivElement = <HTMLDivElement>document.createElement("DIV");
            let res = result.innerHTML;
            res += "<table id='tasksTbl'><tr>"+
            "<th>Id</th>"+
            "<th>Description</th>"+
            "<th>CompleteDate</th>"+
            "<th>Count</th>" +
            "<th>Check</th>"+
            "</tr>";
            Return.innerHTML = result.innerHTML;

            
            response.data.forEach((retask: ReTask) => {
                if (!retask.done)
                {
                    res += "<tr id='task" + retask.trid + "'><td>" + retask.trid + "</td>"
                    + "<td>" + retask.description + "</td>"
                    + "<td>" + retask.completedate + "</td>"
                    + "<td>" + retask.count + "</td>"
                    + "<td><button id='" + retask.trid + "' class='completeReBtn'>Complete</button></td></tr>";
                }
            })
            res += "</table>";

            Return.innerHTML = res;

            var btns = document.getElementsByClassName('completeReBtn');
            for (var i = 0; i < btns.length; i++)
            {
                (function() {
                    var btn = btns[i];
                    var id = btn.id;
                    btn.addEventListener("click", function() { CompleteReTask(id); });
                }());                
            }

            //Return.innerHTML = res;
        })
        .catch(function (error: AxiosError): void {
            Return.innerHTML = "" + error;
        })
}
function DeleteReTask(trid: string): void {
    axios.delete(uri + "taskrepeat/" + trid)
        .then((response: AxiosResponse) => { Return.innerHTML = "response " + response.status + " " + response.statusText; })
        .catch((error: AxiosError) => { Return.innerHTML = ""+error; });
}