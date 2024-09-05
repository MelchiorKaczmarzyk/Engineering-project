import { GameSystem } from "../gameSystem/GameSystemModel"
import { GmForSession } from "../gm/GmForSession"

export default interface ISessionPost {
    system : GameSystem
    title : string
    tags : string
    description : string
    maxNumberOfPlayers : number
    picture: string
    gmUserName : string
}