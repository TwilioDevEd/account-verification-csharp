<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

# Account Verification - ASP.NET MVC

[![Build status](https://ci.appveyor.com/api/projects/status/u7adfy20o1d2mcbt?svg=true)](https://ci.appveyor.com/project/TwilioDevEd/account-verification-csharp)

Learn to implement account verification in your web app with Twilio-powered Authy. Account verification helps you ensure your customer data is accurate and secure. This tutorial will show you the code to make it happen.

[View the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/account-verification/csharp/mvc)!

### Local development

1. First clone this repository and `cd` into it.

   ```shell
   git clone git@github.com:TwilioDevEd/account-verification-csharp.git
   cd account-verification-csharp
   ```

1. Create the sample configuration file and edit it to match your configuration.

  ```shell
  rename AccountVerification.Web\Local.config.example AccountVerification.Web\Local.config
  ```

 You can find your `TwilioAccountSID` and `TwilioAuthToken` in your
 [Twilio Account Settings](https://www.twilio.com/console).

 You will also need a `TwilioNumber`, which you may find
 [here](https://www.twilio.com/user/account/phone-numbers/incoming).

 For the `AuthyKey` you first need to [sign up for Authy](https://dashboard.authy.com/signup).
 When you create an Authy application the production key is found on the dashboard.

 ![Authy Dashboard](http://s3.amazonaws.com/howtodocs/2fa-authy-dashboard.png)

1. Check it out at [http://localhost:25451/](http://localhost:25451/).

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
