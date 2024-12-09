import { Component, OnDestroy, OnInit } from "@angular/core";
import { UserService } from "../../services/user.service";
import { Subscription } from "rxjs";
import { UserModel } from "../../models/User";

@Component({
    selector: "manage-users",
    templateUrl: "manage-users.component.html",
    styleUrls: ["manage-users.component.css"],
})
export class MenageUsers implements OnInit, OnDestroy {
    constructor(private userService : UserService){}

    columnsToDisplay = ["<profilePicture>","Name", "Role", "Discord"]


    deleteUser(){
        this.userService.deleteChosenUser("")
    }
    users : UserModel[] = []
    subUsers? : Subscription
    ngOnInit(): void {
        this.subUsers = this.userService.getUsers().subscribe({
            next: users => {
                this.users = users
            }
        })
    }
    ngOnDestroy(): void {
        this.subUsers?.unsubscribe()
    }


}