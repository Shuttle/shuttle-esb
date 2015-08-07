---
title: Msmq Installation / Activation
layout: api
---
# Windows 7 / 8

1. Open Control Panel.
2. Click **Programs**, and then under **Programs and Features**, click **Turn Windows features on or off**.
   -or-
   Click **Classic View**, double-click **Programs and Features**, and then in the tasks pane, click **Turn Windows features on or off**.
3. Expand **Microsoft Message Queue (MSMQ) Server**, expand **Microsoft Message Queue (MSMQ) Server Core**, and then select the check boxes for the Message Queuing features that you want to install.
4. Click **OK**.
5. If you are prompted to restart the computer, click **OK** to complete the installation.

# Windows Server 2008

1. Click **Start**, point to **Programs**, point to **Administrative Tools**, and then click **Server Manager** to display the Server Manager.
2. Click **Add Features** to start the **Add Features Wizard**.
3. Expand **MSMQ**, expand **MSMQ Services**, and then select the check boxes for the Message Queuing features that you want to install.
4. Click **Next**, and then click **Install**.
5. If you are prompted to restart the computer, click **OK** to complete the installation.

# Windows Server 2012

1. Launch **Server Manager**.
2. Navigate to **Manage** > **Add Roles and Features**.
3. Click **Next** in the **Before You Begin screen**.
4. Select **Role-based or feature-based installation** and click **Next**.
5. Select the server where to install the feature and click **Next**.
6. Do not select any server roles.  Click **Next**.
7. In **Features**, expand **Message Queuing** > **Message Queuing Services** and select **Message Queuing Server**.
8. Continue running the wizard adding any other roles required by MSMQ (if applicable).
9. Click **Install** to start installation.
10. The setup may require a system restart. Click **OK** to complete the installation.


# Links

- [Install Message Queuing](https://technet.microsoft.com/en-us/library/cc730960.aspx)
- [Installing Message Queuing services (MSMQ) on Windows Server 2012](http://support.gfi.com/manuals/en/me2014R2/Content/Administrator/Installation/System_requirements/MSMQ_2012.htm)
