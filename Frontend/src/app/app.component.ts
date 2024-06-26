import { Component, EventEmitter, Output } from "@angular/core";

@Component({
    selector: "app",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"],
})
export class App
{
    isMenuOpened : boolean = false
    toggleMenu() : void
    {
        this.isMenuOpened = !this.isMenuOpened
    }
    clickedOutside() : void
    {
        this.isMenuOpened = false
    }

    showSessionDetails : boolean = false
    onSessionClicked(){
        this.showSessionDetails = true
    }

    burgerClicked : boolean = false
    burgerNotClicked : boolean = true
    onBurgerClicked() {
        this.burgerClicked = !this.burgerClicked
        this.burgerNotClicked = !this.burgerNotClicked
    }

    chosenExplore : boolean = true
    onExploreClicked() {
        this.chosenExplore = true
        this.chosenMySessions = false
        this.chosenAccount = false
        this.chosenCreateSession = false
    }

    chosenMySessions : boolean = false
    onMySessionsClicked() {
        this.chosenExplore = false
        this.chosenMySessions = true
        this.chosenAccount = false
        this.chosenCreateSession = false
    }

    chosenAccount : boolean = false
    onAccountClicked() {
        this.chosenExplore = false
        this.chosenMySessions = false
        this.chosenAccount = true
        this.chosenCreateSession = false
    }

    chosenCreateSession : boolean = false
    onCreateSessionClicked() {
        this.chosenExplore = false
        this.chosenMySessions = false
        this.chosenAccount = false
        this.chosenCreateSession = true

    }
}