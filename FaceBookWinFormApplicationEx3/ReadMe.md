# Facebook Album & Photo Manager

## Project Overview
This project is an extension of an existing Facebook album and photo management application, utilizing **Design Patterns** for clean, modular, and maintainable code.

## Key Features
### 1. GetRandomPhotoFromRandomAlbum
- The system randomly selects an album and a photo from the user's collection.
- The user must guess the date when the photo was taken.
- After selecting a date, the system provides feedback on whether the answer is correct.

### 2. DownloadAlbum
- The user can select an album from their Facebook photos.
- Before downloading, they can preview the album's photos.
- Clicking the download button opens a dialog to choose a save location.

## Implemented Design Patterns
### 1. Strategy (Sorting Photos Before Download)
#### Why This Pattern?
- Allows easy expansion for different sorting methods without modifying existing code.
- Provides high flexibility in sorting strategy selection.

#### Implementation:
- The `ISortPhotosStrategy` interface defines the `Sort` method.
- Four different sorting strategies are implemented in separate classes:
  - `SortByCreationTimeAscending`
  - `SortByCreationTimeDescending`
  - `SortByCommentsAmountAscending`
  - `SortByCommentsAmountDescending`
- The `DownloadAlbumManager` class holds a `m_SortPhotosStrategy` variable, initialized based on the user's choice.

### 2. Observer (Managing Login Status)
#### Why This Pattern?
- Enables automatic UI updates when the login status changes.
- Ensures a clear separation between business logic and presentation.

#### Implementation:
- The `IFacebookObserver` interface defines the `UpdateLoginStatus` method.
- The `SystemManager` class maintains a list of observers (`m_FacebookObservers`).
- `FormLogin` and `FormMain` register as observers and get notified when the login status changes.

### 3. Iterator (Navigating User Posts)
#### Why This Pattern?
- Provides a convenient way to iterate through user posts while maintaining encapsulation.
- Prevents direct exposure of the collection's internal structure.

#### Implementation:
- The `PostCollection` class implements `IEnumerable<Post>`.
- The `PostIterator` inner class implements `IEnumerator<Post>` and manages iteration:
  - `MoveNext()` advances the index.
  - `Reset()` resets the index.
  - `Current` returns the current item.

## How to Use
1. Launch the application.
2. Log in to your Facebook account.
3. Choose one of the available features:
   - **Guess the date of a randomly selected photo.**
   - **Download an album with a customized sorting strategy.**

## UML Diagrams
- **Sequence Diagram** and **Class Diagram** can be found in the `UML_Diagrams` directory.
