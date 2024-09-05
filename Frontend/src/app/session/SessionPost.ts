import { GameSystem } from "../gameSystem/GameSystemModel"
import { GmForSession } from "../gm/GmForSession"
import ISessionPost from "./ISessionPost"

export class SessionPost implements ISessionPost{
    system : GameSystem = new GameSystem()
    title : string = ""
    tags : string = ""
    description : string = ""
    maxNumberOfPlayers : number = 0
    picture: string = ""
    //picture : FormData = new FormData()
    gmUserName : string = ""
}