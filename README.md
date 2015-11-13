# Account Verification
[![Build status](https://ci.appveyor.com/api/projects/status/u7adfy20o1d2mcbt?svg=true)](https://ci.appveyor.com/project/TwilioDevEd/account-verification-csharp)

Use Authy and Twilio to verify your user's account.

### Local development

1. First clone this repository and `cd` into its directory:
   ```
   git clone git@github.com:TwilioDevEd/account-verification-csharp.git

   cd account-verification-csharp
   ```

2. Create a new file AccountVerification.Web/Local.config and update the content with:

   ```
   <appSettings file="Local.config">
        <add key="TwilioAuthToken" value="your_twilio_account_auth_token" />
        <add key="TwilioNumber" value="your_twilio_number" />
        <add key="TwilioNumber" value="your_twilio_number" />
        <add key="AuthyKey" value="your_authy_key" />
   </appSettings>
   ```
3. Hit `Ctrl + F5` to build and run the solution.

4. Check it out at [http://localhost:25451/](http://localhost:25451/)

That's it

