import { PlayerForSession } from "./PlayerForSession"
import { GmForSession } from "./GmForSession"
import { GameSystem } from "./GameSystem"

export class Session
{
    system!: GameSystem
    title: string = ""
    tags: string = ""
    description: string = ""
    gm : GmForSession = new GmForSession()
    players! : PlayerForSession[]
    currentNumberOfPlayers: number = 0
    maxNumberOfPlayers: number = 0
    //New ones
    triggers: string = ""
    date: string = ""
    isRemote : boolean = false
    //Like city or town, locale - (napisz to na froncie)
    location: string = ""
    vtt: string = ""
}