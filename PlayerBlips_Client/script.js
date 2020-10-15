/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
//----------------------------------//
///// VenoX Gaming & Fun 2020 © ///////
//////By Solid_Snake & VnX RL Crew////
////////www.venox-reallife.com////////
//----------------------------------//

import alt from 'alt-client';
import * as game from "natives";

let PlayerList = {};

function CreateBlip(name, pos, sprite, color, shortrange) {
    try {
        let blip = game.addBlipForCoord(pos[0], pos[1], pos[2]);
        game.setBlipAlpha(blip, 255);
        game.setBlipSprite(blip, sprite);
        game.setBlipColour(blip, color);
        game.setBlipAsShortRange(blip, shortrange);
        game.beginTextCommandSetBlipName("STRING");
        game.addTextComponentSubstringPlayerName(name);
        game.endTextCommandSetBlipName(blip);
        alt.log("Blip for Player : " + name + " created!");
        return blip;
    }
    catch { }
}


alt.onServer('PlayerBlips:Update', (Id, Position, Color, Name) => {
    if (!PlayerList[Id])
        return PlayerList[Id] = CreateBlip(Name, [Position.x, Position.y, Position.z], 1, Color, true);
    game.setBlipCoords(PlayerList[Id], Position.x, Position.y, Position.z);
})

alt.onServer('PlayerBlips:Delete', (Id) => {
    if (!PlayerList[Id]) return;
    game.removeBlip(PlayerList[Id]);
})