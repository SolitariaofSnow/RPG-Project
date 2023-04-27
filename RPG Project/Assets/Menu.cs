using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// TODO: using Save = Utils.Save;
// TODO: using Party = ???;
// TODO: using Inventory = ???;

// A menu class for the overworld
public class OverworldMenu : MonoBehaviour {

    private class MenuOption {
        public MenuOption(string name, delegate*<void> select)
            : this(select, name, true) {};
        public static delegate*<void> Select;
        public string name;
        public bool enabled = true;
    }

    void NavigateMenu() { /* TODO */ }
    void DrawMenu() { /* TODO */ }

    void SelectParty() {
        // open party menu
        this.state = MenuState.Party;
        /* TODO
         * This is where the menu stuff actually happens
         */
        this.state = MenuState.Main;
    }
    void SelectInventory() {
        // open inventory menu
        this.state = MenuState.Inventory;
        /* TODO
         * This is where the menu stuff actually happens
         */
        this.state = MenuState.Main;
    }
    void SelectSave() {
        // open save menu
        this.state = MenuState.Save;
        /* TODO
         * This is where the menu stuff actually happens
         */
        this.state = MenuState.Main;
    }
    void SelectLoad() {
        // open load menu
        this.state = MenuState.Load;
        /* TODO
         * This is where the menu stuff actually happens
         */
        this.state = MenuState.Main;
    }
    void SelectQuit() {
        // open quit menu
        this.state = MenuState.Quit;
        // call some quit function
    }
    void SelectClose() {
        // close menu
        this.menuOpen = false;
    }

    private MenuOption[] options = {
        new MenuOption("party", &SelectParty),
        new MenuOption("inventory", &SelectInventory),
        new MenuOption("save", &SelectSave),
        new MenuOption("load", &SelectLoad),
        new MenuOption("quit", &SelectQuit),
        new MenuOption("close", &SelectClose)
    };

    private enum MenuState {
        Main,
        Party,
        Inventory,
        Save,
        Load,
        Quit
    };

    private MenuState state = MenuState.Main;
    private static uint selected = 0;
    private static bool MenuOpen = false;
}
