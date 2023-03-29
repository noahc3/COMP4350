# ThreadIt (COMP4350 Project)

## Core Features

**User Profiles**

Users should be able to create accounts and subsequently login in order to view and interact with threads. Users should also be able to customize their profiles.

**Create And Interact With Threads**

Users should be able to create, edit, and destroy threads. Users should also be able to interact with other threads by upvoting, downvoting, commenting, and sharing.

**Organize Threads**

Users should be able to create and subscribe to spools so that they can see related threads in an organized fashion. Spool creators should have privileges in the spools that they create.

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

Backend C# Files: [EditorConfig](https://editorconfig.org/)
Typescript/CSS/Json: [Prettier](https://prettier.io/docs/en/configuration.html)

## User Stories

**User Profiles**
- As a new user, I want to create an account so that I can begin exploring Threadit and have information saved
  - Acceptance Criteria
    - Given that I want to create an account on Threadit
    - When I follow the account creation process
    - Then I should have successfully created a new account and gain access to Threadit
  - Priority: High
  - Points: 12
- As a returning user, I want to login to my account so that I can view my personalized content
  - Acceptance Criteria
    - Given that I want to login to my existing Threadit account for my personalized content
    - When I enter my username and password to login
    - Then I should be successful in logging into my Threadit account
  - Priority: High
  - Points: 10
- As a logged-in user I want to be able to customize my profile so that I am distinguishable from other users
  - Acceptance Criteria
    - Given that I want to customize my personal Threadit account
    - When I navigate to the customization page and start applying customizations
    - Then I should see these customizations reflected on my user profile
  - Priority: Low
  - Points: 20

**Create And Interact With Threads**
- As a user, I want to be able to create a thread so that I can share my thoughts and views with other users
  - Acceptance Criteria
    - Given I am a user who wants to create a thread
    - When I click the create thread button
    - Then I should be prompted with a thread creation page with the appropriate fields required
  - Priority: High
  - Points: 18
- As a user, I want to be able to edit and delete my own threads so that I can modify what I have previously shared or remove posts that I have previously shared
  - Acceptance Criteria
    - Given I am a user who wants to edit a thread
    - When I click on the edit thread button
    - Then I should be brought to a edit thread page where I can adjust my thread to my liking
  - Priority: Medium
  - Points: 10
- As a user I want to comment on a thread so that I can discuss content with interested parties
  - Acceptance Criteria
    - Given I am a user who wants to comment on a thread
    - When I click on the comment button on a thread
    - Then I should be prompted to write a comment on the thread
  - Priority: High
  - Points: 16
- As a user I want to upvote(stitch) or downvote(rip) a thread so that I can contribute to what others see
  - Acceptance Criteria
    - Given I am a user who wants to upvote or downvote a thread
    - When I click on the upvote or downvote button
    - Then I should expect to see the vote count to change based on input
  - Priority: Medium
  - Points: 10
- As a user I want to share a thread so that I can show other people what I am interested in
  - Acceptance Criteria
    - Given I am a user who wants to share a thread to others
    - When I click on the share button
    - Then I should be given either a link to the thread or a prompt to share directly within Threadit
  - Priority: Low
  - Points: 12

**Organize Threads**
- As a user I want to create a spool so that I can organize related threads
  - Acceptance Criteria
    - Given I am a user who wants to create a spool
    - When I click the create spool button
    - Then I should be redirected to a page where I can add various threads to my spools
  - Priority: High
  - Points: 18
- As a user I want to subscribe (spin) to a spool so that I can stay up to date with related content
  - Acceptance Criteria
    - Given I am a user who wants to subscribe to a spool
    - When I click the subscribe button
    - Then I should expect to see the spool added to my subscriptions and subscription feed
  - Priority: High
  - Points: 16
- As a spool creator, I want to be able to give others special privileges over the spool so that I do not have to manage everything alone
  - Acceptance Criteria
    - Given I am a spool creator who wants to give others special privileges
    - When I promote a user in my spool
    - Then I should see a special notation next to their username and they should have access to elevated privileges
  - Priority: Medium
  - Points: 10

**Home Feed**
- As a user I want to be able to see a personalized feed of threads so that I can browse content easily
  - Acceptance Criteria
    - Given I am a user with a personalized feed
    - When I click into my home page
    - Then I should see my personalized feed of threads from subscribed spools
  - Priority: High
  - Points: 15
- As a user I want to be able to sort and filter my feed so that I can adjust which content I see first
  - Acceptance Criteria
    - Given I am a user who wants to sort and filter their feed
    - When I select sort and filter options on the page
    - Then I should see my feed get sorted and organized in accordance with the selections
  - Priority: Low
  - Points: 10
- As a user, I want to be able to collapse the sidebar so I have more screen space
  - Acceptance Criteria 
    - Given I am a user
    - When I click into my home page and click the arrow on the menu
    - Then the menu should collapse
  - Priority: Low
  - Points: 8

**Spool Suggestions**
- As a user I want to have spools suggested to me based on my interests so that I can subscribe to new spools
  - Acceptance Criteria
    - Given I am a user who wants to get suggested spools based on my interests
    - When I am browsing Threadit
    - Then I should expect to see accurate suggestions on spools based on my interests
  - Priority: Low
  - Points: 10
- As a user, I want to complete a short quiz about my interests to allow my recommended spools to be relevant to me.
  - Acceptance Criteria
    - Given I am a first time user of Threadit
    - When I first log into Threadit
    - Then I should be prompted with a quick quiz to gauge my interests for suggestions, and then see suggestions based off the answers given
  - Priority: Low
  - Points: 15
