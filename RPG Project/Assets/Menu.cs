using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// TODO: using Save = Utils.Save;
// TODO: using Party = ???;
// TODO: using Inventory = ???;

// A menu class for the overworld
public unsafe class OverworldMenu : MonoBehaviour {
    private unsafe class MenuOption {
        public MenuOption(string name, delegate*<void> select){}
        public delegate*<void> Select;
        public string name;
        public bool enabled = true;
    }

    void NavigateMenu() { /* TODO */ }
    void DrawMenu() { /* TODO */ }

    static void SelectParty() {
        // open party menu
        state = MenuState.Party;
        /* TODO
         * This is where the menu stuff actually happens
         */
        state = MenuState.Main;
    }
    static void SelectInventory() {
        // open inventory menu
        state = MenuState.Inventory;
        /* TODO
         * This is where the menu stuff actually happens
         */
        state = MenuState.Main;
    }
    static void SelectSave() {
        // open save menu
        state = MenuState.Save;

        // TODO: Are you sure you want to save?

        SaveDataManager.Save(Config.SaveFile);

        state = MenuState.Main;
    }
    static void SelectLoad() {
        // open load menu
        state = MenuState.Load;
        // TODO: Are you sure you want to load?

        SaveDataManager.Load(Config.SaveFile);

        state = MenuState.Main;
    }
    static void SelectQuit() {
        // open quit menu
        state = MenuState.Quit;
        // call some quit function
    }
    static void SelectClose() {
        // close menu
        MenuOpen = false;
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

    private static MenuState state = MenuState.Main;
    private static uint selected = 0;
    private static bool MenuOpen = false;
}
