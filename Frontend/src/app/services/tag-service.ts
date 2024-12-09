import { HttpClient } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { Observable, tap } from "rxjs"
import { Session } from "../models/Session"
import { Tag } from "../models/Tag"


@Injectable({
    providedIn: 'root'
})
export class TagsService {
    //lokalny ip, musisz go sprawdzić ipconfig przed prezentacją
    private url = "https://localhost:7271/api/tags/tagService"

    constructor(private http: HttpClient) {}
        getTags(): Observable<Tag[]>{
        return this.http.get<Tag[]>(this.url).pipe(
            tap(data => console.log("All", JSON.stringify(data)))
        )
}
}
