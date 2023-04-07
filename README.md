# ThreadIt (COMP4350 Project)

## Core Features

**User Profiles**

Users should be able to create accounts and subsequently login in order to view and interact with threads. Users should also be able to customize their profiles.

**Create And Interact With Threads**

Users should be able to create, edit, and destroy threads. Users should also be able to interact with other threads by upvoting, downvoting, commenting, and sharing.

**Organize Threads**

Users should be able to create and subscribe/join to spools so that they can see related threads in an organized fashion. Spool creators should have privileges in the spools that they create.

**Home Feed**

Users should be able to see a personalized feed containing threads from spools that they are subscribed to. Users should be able to sort their feed to maximize their browsing experience.

**Spool Suggestions (optional)**

Users should have spools suggested to them based on the spools that they are already subscribed to. When a user first creates an account they should receive spool suggestions based on their interests.

**Non-functional Feature**

This application should be able to respond to 1000 requests across 100 users per minute concurrently.

## Technologies
- .NET: Backend REST API
- React.js: Web Frontend
- Postgres: Database
- GitHub Actions: CI/CD
- Jira: Project Management

## Coding Style

We are using [Prettier](https://prettier.io/docs/en/configuration.html) to format Typescript, CSS, and Json files, and [EditorConfig](https://editorconfig.org/) to format C# files. 

Prettier Default Formatting Rules (Front-end)
- 2 space indentation
- Max 80 character line length
- Single quotes
- Remove unnecessary semi-colons
- Remove unnecessary whitespace
- Use arrow functions by default

EditorConfig Default Formatting Rules (Back-end C#)
- 4 space indentation
- Max 80 character line length
- Insert final newline
- Disallow trailing whitespace
- Unix-style line endings
- utf-8 character set