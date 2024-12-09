import { HttpClient } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { Observable, tap } from "rxjs"
import { Session } from "../models/Session"
import { Trigger } from "../models/Trigger"


@Injectable({
    providedIn: 'root'
})
export class TriggersService {
    //lokalny ip, musisz go sprawdzić ipconfig przed prezentacją
    private url = "https://localhost:7271/api/triggers/triggerService"

    constructor(private http: HttpClient) {}
        getTriggers(): Observable<Trigger[]>{
        return this.http.get<Trigger[]>(this.url).pipe(
            tap(data => console.log("All", JSON.stringify(data)))
        )
}
}
