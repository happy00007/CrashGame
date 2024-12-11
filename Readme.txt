Crash Game - Unity Project
Overview
This crash game is built in Unity and allows users to engage in dynamic and thrilling gameplay. This README serves as a guide to help developers modify and extend the functionality of the game. Below, you will find detailed information about the project's structure, scripts, and how to make adjustments.

Unity Version: 2023.2.8f1
Language: C#

Project Structure
The project is organized into several key scripts that manage different aspects of the game:
Core Scripts
* ConnectionManager.cs: Handles player connection and network management.
* GameManager.cs: Manages the overall game lifecycle and global game state.
* GamePlayHandler.cs: Contains the logic for gameplay, such as managing crash mechanics.
* GameResetManager.cs: Resets the game state after each round.
* GameStartManager.cs: Handles pre-game setup and initialization.
* UIManager.cs: Manages the game's UI elements and interactions.
Player Management
* PlayerInfo.cs: Stores player-specific information.
* PlayerLogin.cs: Manages player authentication and login logic.
* PlayerState.cs: Keeps track of the player's current state in the game.
Networking and Rooms
* RoomManager.cs: Handles the creation and management of game rooms.
* RoomNPlayerState.cs: Synchronizes player state within a room.
* RoomStateManager.cs: Manages the state of game rooms.
Leaderboards and Betting
* LeaderBoardHandler.cs: Updates and displays leaderboard data.
* LeaderboardItemDetail.cs: Represents individual leaderboard entries.
* BettingManager.cs: Handles player betting functionality.
Utilities
* APIStrings.cs: Defines string constants for API endpoints.
* AllCustomProperties.cs: Manages custom properties for various objects.
* DestroyAfterDelayy.cs: Handles timed destruction of game objects.
* GetJson.cs: Facilitates JSON parsing for external data.
* LocalSettings.cs: Manages local game settings.
* OrientationHandler.cs: Adjusts game behavior based on device orientation.
* ScaleBuilder.cs: Dynamically scales UI and game objects.
* ShareOnTwitter.cs: Enables players to share their progress on Twitter.
* WalletManager.cs: Manages player wallet and in-game currency.

How to Modify the Game
1. Game Settings
* Modify game settings in LocalSettings.cs.
* Configure API strings in APIStrings.cs for backend integration.
2. Gameplay Mechanics
* Adjust crash mechanics and round logic in GamePlayHandler.cs.
* Customize betting functionality in BettingManager.cs.
* Modify room behaviors in RoomManager.cs and RoomStateManager.cs.
3. UI Customization
* Update UI elements and layout in UIManager.cs.
* Use ScaleBuilder.cs to adjust X and Y axis scale of the game dynamically.
4. Networking
* Use ConnectionManager.cs to integrate with new networking solutions.
* Modify room and player synchronization logic in RoomNPlayerState.cs.
5. Player Features
* Add new player features or stats in PlayerInfo.cs.
* Enhance login mechanisms in PlayerLogin.cs.
6. Leaderboards
* Update leaderboard logic in LeaderBoardHandler.cs.
* Modify leaderboard data representation in LeaderboardItemDetail.cs.

Dependencies
Ensure the following dependencies are installed for smooth functionality:
* Unity Version: 2023.2.8f1.
* DOTween: Used for animations and transitions.
* Newtonsoft.Json: For JSON parsing.

Tips and Best Practices
* Maintain consistent naming conventions for new scripts and variables.
* Test changes in a separate branch to avoid disrupting the main codebase.
* Use Unity's built-in profiler to optimize performance.
* Regularly back up your project.

Troubleshooting
Common Issues
1. Game not starting:
o Ensure all required components are added to the scene.
o Check initialization logic in GameStartManager.cs.
2. Leaderboard not updating:
o Verify API integration in LeaderBoardHandler.cs.
o Ensure proper data synchronization.
3. UI scaling issues:
o Adjust scaling parameters in OrientationHandler.cs**.

Contact
For further assistance or to report issues, please contact the development team.


