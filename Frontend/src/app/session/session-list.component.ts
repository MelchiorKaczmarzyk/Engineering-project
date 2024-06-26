import { Component, OnDestroy, OnInit } from "@angular/core";
import { SessionService } from "./session-service";
import {  Subscription } from "rxjs";
import { Session } from "./Session";

@Component({
    selector: "session-list",
    templateUrl: "./session-list.component.html",
    styleUrl: "./session-list.component.css"
})
export class SessionList implements OnInit, OnDestroy {
    constructor(private sessionService : SessionService) {}
    private _filterGm = ""
    get filterGm() : string {
        return this._filterGm
    }
    set filterGm(value : string) {
        this._filterGm = value
    }
    private _filterSystem = ""
    get filterSystem() : string {
        return this._filterSystem
    }
    set filterSystem(value : string) {
        this._filterSystem = value
    }
    private _filterTag = ""
    get filterTag() : string {
        return this._filterTag
    }
    set filterTag(value : string) {
        this._filterTag = value
    }

    filtersGm : string[] = []
    filtersSystem : string[] = []
    filtersTags : string[] = []

    filtersDisplayedGm : string = ""
    fitlersDisplayedSystem : string = ""
    filtersDisplayedTags : string = ""
    
    onAddGmFilterClicked()
    {
        if(this.filtersDisplayedGm.length != 0)
            {
                this.filtersDisplayedGm += ", "
            }
        this.filtersDisplayedGm += this.filterGm
        this.filterGm = ""
        
        this.filtersGm = this.filtersDisplayedGm.split(",")
        this.filteredSessions = this.filterSessions()
    }
    onAddSystemFilterClicked()
    {
        if(this.fitlersDisplayedSystem.length != 0)
            {
                this.fitlersDisplayedSystem += ", "
            }
        this.fitlersDisplayedSystem += this.filterSystem
        this.filterSystem = ""
        
        this.filtersSystem = this.fitlersDisplayedSystem.split(",")
        this.filteredSessions = this.filterSessions()
    }
    onAddTagFilterClicked()
    {
        if(this.filtersDisplayedTags.length != 0)
            {
                this.filtersDisplayedTags += ", "
            }
        this.filtersDisplayedTags += this.filterTag
        this.filterTag = ""
        
        this.filtersTags = this.filtersDisplayedTags.split(",")
        this.filteredSessions = this.filterSessions()
    }

    onClearGmFiltersClicked()
    {
        this.filterGm = ""
        this.filtersGm = []
        this.filtersDisplayedGm = ""
        this.filteredSessions = this.filterSessions()
    }
    onClearSystemFiltersClicked()
    {
        this.filterSystem = ""
        this.filtersSystem = []
        this.fitlersDisplayedSystem = ""
        this.filteredSessions = this.filterSessions()
    }
    onClearTagFiltersClicked()
    {
        this.filterTag = ""
        this.filtersTags = []
        this.filtersDisplayedTags = ""
        this.filteredSessions = this.filterSessions()
    }

    sessions : Session[] = []
    filteredSessions : Session[] = []
    private compareTags(session : Session, tags : string[]) : boolean{
        if(tags.length == 0)
            return true
        else{
        var sessionTags = session.tags.split(",")
        return sessionTags.some(tag => tags.includes(tag))
        }
    }

    filterSessions() : Session[]{
        return this.sessions.filter((session) => (this.filtersDisplayedGm
        .includes(session.gm.name) || this.filtersDisplayedGm == "") && (
            this.fitlersDisplayedSystem
            .includes(session.system) || this.fitlersDisplayedSystem == "") && (
                this.compareTags(session, this.filtersTags)))
         //session.gm.name.includes(this.filterGm))
    }

    sub!: Subscription
    ngOnInit(): void {
        console.log("use this method to, for example, retrive data from a backend")
         this.sub = this.sessionService.getSessions().subscribe({
             next: sessions => {
                 this.sessions = sessions
                 this.filteredSessions = this.sessions
             }
         })
     }
     ngOnDestroy(): void {
         this.sub.unsubscribe
     }
}