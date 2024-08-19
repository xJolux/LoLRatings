# LoL Ratings

**LoL Ratings** is a tool that provides real-time performance metrics for League of Legends players. It uses live game data to evaluate player performance and updates automatically every 3 seconds.

## Installation

1. Download [LoLRatingsInstaller.exe](https://github.com/xJolux/LoLRatings/releases/latest).
2. Run the installer and follow the on-screen instructions.

## Usage

1. Launch the application.
2. The UI will automatically display live game statistics.

## UI Details

### General
- **Window**: Transparent, located in the top-left corner of the screen. You can click through the window with your mouse.
- **Design**: Some elements are color-coded according to team colors.
### Stats
- **Win %**: Shows win percentages for Blue and Red teams.
- **Rating % Diff**: Displays the difference between the percentage of total rating points for the Blue and Red teams. For example, if Blue has 60% and Red has 40% of the total rating points, the Rating % Diff will be 20%. Note that this is not the same as the Win %.
- **End Time**: Estimated total duration of the match in minutes/seconds. Note that this estimate is not highly accurate and should be taken with caution.
- **Player Stats**:
  - **Players**: Lists champion names and player roles.
  - **#**: Player ranking (1-10).
  - **Min-Max**: Performance rating from the worst to the best player (0-100).
  - **Max%**: Player performance relative to the best player (0-100).
  - **Perf.**: Player performance compared to the average (0-100 * number of players).

## Data Storage

At the end of a match, the Rating % Diff and match duration are saved to improve end time predictions. This data is stored locally in a file found in the `Data` folder within the program directory.
