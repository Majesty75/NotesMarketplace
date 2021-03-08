## Backend Submision 1
### Completed:
    #### 3.1 Login, Signup, Forgot password, Change password and Email Verification
    #### 3.2 Save and Publish Note, Note Deatils (Yet to complete Download Button Flow for registred user.)
    #### 3.3 User Dashboard aka Sell Your Notes is yet static(included in next submission as 3.7.1 my profile).
      [x] Add Notes Button Works
      [x] Buyer Request tab in statistics redirect BuyerRequests Page.
      [x] Header and Footer works.
      [ ] Everything Other than them is static
    #### 3.4 Buyer Request Completed.
    #### 4.1 When Super Admin Log in it redirect them to admin dashboard (static).
    #### 2.2 Search Notes Completed Search and Filter works. 
    #### 2.5 and 2.6 Contact Us and Login completed
    
    
    
### UnderStanding UserRoles
- Super Admin (Userole ID: 1) : Login in and will be redirected to Admin Static DashBoard. (They can access anonymous pages such as home,faq,contact us as loged in users.)

- Sub Admin (Userole ID: 2) : Same as super admin

- Registred User (Userole ID: 3) : User which has his UserProfile created and can access all of the above mentioned completed pages.

- UserProfileNotCreated (Userole ID: 4) : New user made with sign up will not have any option cause he will be redirected to user profile if he access any other pages except anonymous pages.
  *there is no backend for user profile in this submission. so to make user normal change role ID to 3 in database.*
  
- Anonymous Users : Can Access Index (Home), Contact Us, Search Notes, FAQ Pages.

### Crendentials Already Into System
* superadmin@gmail.com   Password: \@Toornimda1 will be redirected to Static Admin Dashboard. 
* subadmin@gmail.com     Password: PassWord2 : it is normal user right now so it will word as normal registered user
* dummymail@gmail.com    Password: Password1 : UserprofileNotCraetedUser.
* yashgoyani101@gmail.com Password: P@sswd1 : UserprofileNotCreatedUser.

This Are Passwords changed using DB, passwords on signup requires to be according to SRS. 
