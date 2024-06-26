import { GmForSession } from "../gm/GmForSession"

export default interface ISessionPost {
    system : string
    title : string
    tags : string
    description : string
    maxNumberOfPlayers : number
}