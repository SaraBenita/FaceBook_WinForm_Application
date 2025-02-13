# Facebook Desktop App - Design Patterns Documentation

## Introduction
This project is a desktop application that integrates with the **Facebook API**, allowing users to interact with their photo albums and play a guessing game. Below is an explanation of the **Design Patterns** used in the implementation and how they improve the project.

---

## Design Patterns Used

### 1. Proxy Pattern
#### **Why was it chosen?**
The **Proxy Pattern** was used to optimize performance and prevent multiple direct queries to the **Facebook API** (User object from the SDK). Instead of fetching user data repeatedly from the network, we cache necessary information locally, reducing API calls and improving response time.

#### **Implementation**
- **Class:** `UserCashingProxy`
- **How it works:**
  - Instead of directly interacting with the Facebook `User` object, the system interacts with `UserCashingProxy`.
  - `UserCashingProxy` caches user-related data (groups, posts, photos) to reduce redundant API calls.
  - `UserAdapter` acts as an intermediary, ensuring compatibility with the existing Facebook SDK.
  - `IUserAdapter` interface is implemented by both `UserAdapter` and `UserCashingProxy`.

#### **Code Structure:**
- `UserCashingProxy` – Manages caching logic.
- `SystemManager` – Serves as a mediator for the system.
- `UserAdapter` – Acts as an adapter for `User`.
- `IUserAdapter` – Interface for `UserAdapter` and `UserCashingProxy`.

**Benefits:**
- Reduces network dependency and enhances performance.
- Ensures smoother user experience with cached data.
- Avoids unnecessary API requests, reducing rate limits risks.

---

### 2. Factory Method Pattern
#### **Why was it chosen?**
The **Factory Method Pattern** was used to centralize and standardize the creation of UI forms (`FormLogin`, `FormMain`, etc.). This allows consistent object creation, improves maintainability, and simplifies the client code.

#### **Implementation**
- **Class:** `FormFactory`
- **How it works:**
  - `FormFactory` is a static class responsible for creating form instances.
  - `createFormBasedOnType()` is a static method that takes an `eFormType` enum and returns the corresponding form instance.

#### **Code Structure:**
- `FormFactory.createFormBasedOnType(FormFactory.eFormType.FormLogin)` – Used in `Program.cs` for initializing the application.
- `FormFactory.createFormBasedOnType(FormFactory.eFormType.FormMain)` – Used in `FormLogin` after successful authentication.

**Benefits:**
- Centralized logic for form creation ensures consistency.
- Simplifies client code – no need to instantiate forms manually.
- Improves reusability and maintainability.

---

### 3. Singleton Pattern
#### **Why was it chosen?**
The **Singleton Pattern** was used to ensure that only one instance of `GuessTheMomentManager` exists throughout the application's lifetime.

#### **Implementation**
- **Class:** `GuessTheMomentManager`
- **How it works:**
  - A private static variable (`s_GuessTheMomentInstance`) holds the single instance.
  - A private constructor prevents instantiation from outside the class.
  - A public static property (`GuessTheMomentInstance`) provides access to the instance.
  - Double-checked locking (`sr_DoubleCheckLock`) ensures thread safety.

#### **Code Structure:**
- `GuessTheMomentManager` is accessed in `FormMain.cs` within UI event handlers like `buttonGuessTheMoment_Click()`.

**Benefits:**
- Saves memory by ensuring a single instance.
- Avoids race conditions when accessing shared resources.
- Provides a single source of truth for game logic.

---

## Additional Implementations

### **Data Binding**
- Used for binding Facebook page data to UI components (`ListBox` and `PictureBox`).
- `pageBindingSource.DataSource = pages;` ensures automatic UI updates when data changes.
- Reduces manual UI updates, improves performance, and maintains separation of concerns.

### **Asynchronous Programming**
- Used for loading user data, posts, pages, and albums to keep the UI responsive.
- Implemented with background threads and `Invoke` to update UI components safely.
- Enhances user experience by preventing UI freezes during data retrieval.

---

## Summary
The use of **Proxy, Factory Method, and Singleton** patterns significantly improves:
- **Performance** – By caching API responses and reducing network calls.
- **Code Maintainability** – By standardizing object creation and ensuring a single source of truth.
- **User Experience** – By using asynchronous programming and efficient data binding.

This design ensures a **scalable, maintainable, and high-performance** desktop application for interacting with Facebook.
