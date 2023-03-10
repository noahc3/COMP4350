# Project Proposal

**Project Name:** Threadit  
**Instructor:** Shaowei Wang  
**TA:** Ridham Shah  
**Group Name:** AnonCo  
**Group Members:**
- Nathan Poppe
- Anthony Phung
- Cody Wallbridge
- Noah Curoe

## Vision

Introducing **Threadit**, the newest social media platform designed to share ideas, engage in thoughtful discussions, and connect with others throughout the world while fostering an environment of mutual understanding and personal growth. Spanning a wide variety of communities including sports, entertainment, and politics, Threadit is the perfect destination for you to stay connected, discover new perspectives and grow as a person.

## Project Summary

**Threadit** is a social media platform that aims to revolutionize online communication by creating a community-driven platform for in-depth discussions and meaningful conversations. It is built around organized and specific threads called "spools" where users can discover and connect with like-minded individuals on a wide variety of topics. The platform also features a unique "stitch" system that helps members quickly and easily see the best content and a "rip" system for downvoting posts that do not measure up. The vision of Threadit is to empower individuals to connect, engage and share knowledge in a community-driven platform that fosters mutual understanding and personal growth.

## Stakeholders

Threadit is the perfect platform for marketing teams to create communities surrounding their products in order to generate content to engage customers and increase excitement about their product. Threadit is a great place for fan bases to come together and discuss their favourite team, band, or video game. Threadit allows students to ask questions about certain subjects and receive answers from experts. Overall Threadit is the place to be for everyone to learn, explore and interact with others.

The stakeholders of Threadit would likely include:

- The users or members of the platform, as they are the primary audience and directly benefit from the platform's features and community.
- The platform's owners or operators, as they have a vested interest in the success and growth of the platform.
- Advertisers or partners, as they may be interested in reaching Threadit's audience or potentially monetizing the platform in some way.
- Investors, as they have invested in the platform and have a stake in its success.
- The community managers, as they may be responsible for moderating the platform and ensuring its content is appropriate.
- The employees of the company, as they have invested their time and effort to build the platform and are dependent on the platform's success for their own livelihood. 

## Core Features

### User Profiles

Users should be able to create accounts and subsequently login in order to view and interact with threads. Users should also be able to customize their profiles.

### Create And Interact With Threads

Users should be able to create, edit, and destroy threads. Users should also be able to interact with other threads by upvoting, downvoting, commenting, and sharing.

### Organize Threads

Users should be able to create and subscribe to spools so that they can see related threads in an organized fashion. Spool creators should have privileges in the spools that they create.

### Home Feed

Users should be able to see a personalized feed containing threads from spools that they are subscribed to. Users should be able to sort their feed to maximize their browsing experience.

### Spool Suggestions

Users should have spools suggested to them based on the spools that they are already subscribed to. When a user first creates an account they should receive spool suggestions based on their interests.

### Non-functional Feature

This application should be able to respond to 1000 requests across 100 users per minute concurrently.

## Technologies

- .NET: Backend REST API
- React.js: Web Frontend
- Postgres: Database
- GitHub Actions: CI/CD
- Jira: Project Management

## User Stories

### User Profiles

1. As a new user, I want to create an account so that I can begin exploring Threadit and have information saved  
    AC: Given that I want to create an account on Threadit   
        When I follow the account creation process  
        Then I should have successfully created a new account and gain access to Threadit  
    Priority: High  
    Points: 12  
2. As a returning user, I want to login to my account so that I can view my personalized content  
    AC: Given that I want to login to my existing Threadit account for my personalized content  
        When I enter my username and password to login  
        Then I should be successful in logging into my Threadit account  
    Priority: High  
    Points: 10  
3. As a logged-in user I want to be able to customize my profile so that I am distinguishable from other users  
    AC: Given that I want to customize my personal Threadit account  
        When I navigate to the customization page and start applying customizations  
        Then I should see these customizations reflected on my user profile  
    Priority: Low  
    Points: 20  

### Create And Interact With Threads

1. As a user, I want to be able to create a thread so that I can share my thoughts and views with other users  
    AC: Given I am a user who wants to create a thread  
        When I click the create thread button  
        Then I should be prompted with a thread creation page with the appropriate fields required  
    Priority: High  
    Points: 18  
2. As a user, I want to be able to edit and delete my own threads so that I can modify what I have previously shared or remove posts that I  have previously shared  
    AC: Given I am a user who wants to edit a thread      
        When I click on the edit thread button  
        Then I should be brought to a edit thread page where I can adjust my thread to my liking  
    Priority: Medium  
    Points: 10  
3. As a user I want to comment on a thread so that I can discuss content with interested parties  
    AC: Given I am a user who wants to comment on a thread  
        When I click on the comment button on a thread  
        Then I should be prompted to write a comment on the thread  
    Priority: High  
    Points: 16  
4. As a user I want to upvote(stitch) or downvote(rip) a thread so that I can contribute to what others see  
    AC: Given I am a user who wants to upvote or downvote a thread  
        When I click on the upvote or downvote button  
        Then I should expect to see the vote count to change based on input  
    Priority: Medium  
    Points: 10  
5. As a user I want to share a thread so that I can show other people what I am interested in  
    AC: Given I am a user who wants to share a thread to others  
        When I click on the share button  
        Then I should be given either a link to the thread or a prompt to share directly within Threadit  
    Priority: Low  
    Points: 12  

### Organize Threads

1. As a user I want to create a spool so that I can organize related threads    
    AC: Given I am a user who wants to create a spool    
        When I click the create spool button  
        Then I should be redirected to a page where I can add various threads to my spools  
    Priority: High  
    Points: 18  
2. As a user I want to subscribe (spin) to a spool so that I can stay up to date with related content  
    AC: Given I am a user who wants to subscribe to a spool  
        When I click the subscribe button  
        Then I should expect to see the spool added to my subscriptions and subscription feed  
    Priority: High  
    Points: 16  
3. As a spool creator, I want to be able to give others special privileges over the spool so that I do not have to manage everything alone  
    AC: Given I am a spool creator who wants to give others special privileges  
        When I promote a user in my spool  
        Then I should see a special notation next to their username and they should have access to elevated privileges  
    Priority: Medium  
    Points: 10  

### Home Feed

1. As a user I want to be able to see a personalized feed of threads so that I can browse content easily  
    AC: Given I am a user with a personalized feed  
        When I click into my home page  
        Then I should see my personalized feed of threads from subscribed spools  
    Priority: High  
    Points: 15  
2. As a user I want to be able to sort and filter my feed so that I can adjust which content I see first  
    AC: Given I am a user who wants to sort and filter their feed  
        When I select sort and filter options on the page  
        Then I should see my feed get sorted and organized in accordance with the selections  
    Priority: Low  
    Points: 10  
3. As a user, I want to be able to collapse the sidebar so I have more screen space    
    AC: Given I am a user   
        When I click into my home page and click the arrow on the menu  
        the menu should collapse  
        Priority: Low  
        Points: 8  

### Spool Suggestions

1. As a user I want to have spools suggested to me based on my interests so that I can subscribe to new spools  
    AC: Given I am a user who wants to get suggested spools based on my interests  
        When I am browsing Threadit  
        Then I should expect to see accurate suggestions on spools based on my interests  
    Priority: Low  
    Points: 10  
2. As a developer, I want users to complete a simple quiz to populate suggested spools with options  
    AC: Given I am a first time user of Threadit  
        When I first log into Threadit  
        Then I should be prompted with a quick quiz to gauge my interests for suggestions, and then see suggestions based off the answers given  
    Priority: Low  
    Points: 15  
