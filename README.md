# Account Verification - ASP.NET MVC

[![Build status](https://ci.appveyor.com/api/projects/status/u7adfy20o1d2mcbt?svg=true)](https://ci.appveyor.com/project/TwilioDevEd/account-verification-csharp)

Use Authy and Twilio to verify your user's account. [View the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/account-verification/csharp/mvc)!

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
 [Twilio Account Settings](https://www.twilio.com/user/account/settings).
 You will also need a `TwilioNumber`, which you may find [here](https://www.twilio.com/user/account/phone-numbers/incoming).

1. Expose your application to the wider internet using [ngrok](http://ngrok.com). This step
  is important because the application won't work as expected if you run it through
  localhost.

  To start using `ngrok` in our project you'll have execute to the following line in the _command prompt_.

  ```shell
  ngrok http 25451 -host-header="localhost:25451"
  ```

  Keep in mind that our endpoint is:

  ```
  http://<your-ngrok-subdomain>.ngrok.io/Conference/ConnectClient
  ```

1. Check it out at [http://localhost:25451/](http://localhost:25451/).

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.

