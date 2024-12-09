import { HttpClient } from "@angular/common/http";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { GameSystenService } from "../../services/game-system.service";
import { UserService } from "../../services/user.service";
import { TagsService } from "../../services/tag-service";
import { TriggersService } from "../../services/trigger-service";
import { VttsService } from "../../services/vtt.service";
import { Subscription } from "rxjs";
import { GameSystem } from "../../models/GameSystem";
import { Tag } from "../../models/Tag";
import { Trigger } from "../../models/Trigger";
import { Vtt } from "../../models/Vtt";

@Component({
    selector: "session-parameters",
    templateUrl: "session-parameters.component.html",
    styleUrls: ["session-parameters.component.css"],
})
export class SessionParameters implements OnInit, OnDestroy{

    constructor(private http: HttpClient, 
        private systemService: GameSystenService,
        private tagsService : TagsService,
        private triggersService : TriggersService,
        private vttsService : VttsService) { }

    systemToAdd : string = ""
    systemToRemove : string = ""
    previousAddSystem: string = ""
    previousRemoveSystem: string = ""

    addedSystem : boolean = false
    removedSystem: boolean = false
    emptyAddSystem: boolean = false
    emptyRemoveSystem: boolean = false

    tagToAdd : string = ""
    tagToRemove : string = ""
    previousAddTag: string = ""
    previousRemoveTag: string = ""

    addedTag : boolean = false
    removedTag: boolean = false
    emptyAddTag: boolean = false
    emptyRemoveTag: boolean = false

    triggerToAdd : string = ""
    triggerToRemove : string = ""
    previousAddTrigger: string = ""
    previousRemoveTrigger: string = ""

    addedTrigger : boolean = false
    removedTrigger: boolean = false
    emptyAddTrigger: boolean = false
    emptyRemoveTrigger: boolean = false

    vttToAdd : string = ""
    vttToRemove : string = ""
    previousAddVtt: string = ""
    previousRemoveVtt: string = ""

    addedVtt : boolean = false
    removedVtt: boolean = false
    emptyAddVtt: boolean = false
    emptyRemoveVtt : boolean = false

    
    addSystemUrl : string = 'https://localhost:7271/api/gameSystems/addSystem'
    removeSystemUrl : string = 'https://localhost:7271/api/gameSystems/deleteSystem'
    addSystem() {
        this.http.post(this.addSystemUrl, {name : this.systemToAdd}).subscribe({
            complete: () => {
                this.addedSystem = true
                this.previousAddSystem = this.systemToAdd
            },
            error: () => {
                //widok, że error
            }
        })
    }
    removeSystem() {
        this.http.delete(this.removeSystemUrl, {
            params: {
                systemName : this.systemToRemove
            }
        }).subscribe({
            complete: () => {
                this.removedSystem = true
                this.previousRemoveSystem = this.systemToRemove
            },
            error: () => {
                //widok, że error
            }
        })
    }
    addTagUrl : string = 'https://localhost:7271/api/tags/addTag'
    removeTagUrl : string = 'https://localhost:7271/api/tags/deleteTag'
    addTag() {
        this.http.post(this.addTagUrl, {name : this.tagToAdd}).subscribe({
            complete: () => {
                this.addedTag = true
                this.previousAddTag = this.vttToAdd
            },
            error: () => {
                //widok, że error
            }
        })
    }
    removeTag() {
        this.http.delete(this.removeTagUrl, {
            params: {
                tagName : this.tagToRemove
            }
        }).subscribe({
            complete: () => {
                this.removedTag = true
                this.previousRemoveTag = this.tagToRemove
            },
            error: () => {
                //widok, że error
            }
        })
    }
    addTriggerUrl : string = 'https://localhost:7271/api/triggers/addTrigger'
    removeTriggerUrl : string = 'https://localhost:7271/api/triggers/deleteTrigger'
    addTrigger() {
        this.http.post(this.addTriggerUrl, {name : this.triggerToAdd}).subscribe({
            complete: () => {
                this.addedTrigger = true
                this.previousAddTrigger= this.vttToAdd
            },
            error: () => {
                //widok, że error
            }
        })
    }
    removeTrigger() {
        this.http.delete(this.removeTriggerUrl, {
            params: {
                triggerName : this.triggerToRemove
            }
        }).subscribe({
            complete: () => {
                this.removedTrigger = true
                this.previousRemoveTrigger = this.triggerToRemove
            },
            error: () => {
                //widok, że error
            }
        })
    }
    addVttUrl : string = 'https://localhost:7271/api/vtts/addVtt'
    removeVttUrl : string = 'https://localhost:7271/api/tags/deleteVtt'
    addVtt() {
        this.http.post(this.addVttUrl, {name : this.vttToAdd}).subscribe({
            complete: () => {
                this.addedVtt = true
                this.previousAddVtt = this.vttToAdd
            },
            error: () => {
                //widok, że error
            }
        })
    }
    removeVtt() {

    }

    subSystems? : Subscription
    systems : GameSystem[] = []
    subTags? : Subscription
    tags : Tag[] = []
    subTriggers? : Subscription
    triggers : Trigger[] = []
    subVtts? : Subscription
    vtts : Vtt[] = []

    ngOnInit(): void {
        this.subSystems = this.systemService.getSystems().subscribe({
            next: systems => {
                this.systems = systems
            }
        })
        this.subTags = this.tagsService.getTags().subscribe({
            next: tags => {
                this.tags = tags
            }
        })
        this.subTriggers = this.triggersService.getTriggers().subscribe({
            next: triggers => {
                this.triggers = triggers
            }
        })
        this.vttsService.getVtts().subscribe({
            next: vtts => {
            this.vtts = vtts
            }
        })
    }
    ngOnDestroy(): void {
        this.subSystems?.unsubscribe()
        this.subTags?.unsubscribe()
        this.subTriggers?.unsubscribe()
        this.subVtts?.unsubscribe()
    }
}