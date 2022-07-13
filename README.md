# AntiSabotage
AntiSabotage is a EXILED SCP:SL plugin which can protect the server from sabotaging. Also this plugin can track admin commands and send it using Discord webhooks.
# Config
```DRS:
AntiSabotage:
  # Whether or not this plugin is enabled
  is_enabled: true
  # How many seconds will be the period of watching bans after starting banning
  ban_timeout: 20
  # How many bans the administator can give. If they ban more that it their badge will be removed
  ban_limit: 5
  # How many seconds will be the period of watching kicks after starting kicking
  kick_timeout: 20
  # How many kicks the administator can give. If they kick more that it their badge will be removed
  kick_limit: 5
  # URL of webhook
  webhook_url: ''
  # Roles or users to be pinged when sabotaging or using notified command
  pinged_people:
  - <@&981638149572821062>
  - <@510752968551825409>
  # Commands to be notified using webhook
  notified_commands:
  - noclip
  should_notified_command_pinged_people: true
  # Note that the colour has to start with #
  embed_colour: '#ff4040'
  ```
# Translation
```DRS:
AntiSabotage:
  notified_broadcast: <color=red>Your badge have been removed from sabotaging this server. Please contact with your manager.</color>
  attention_text: 'The administrator tried to sabotage the server. To give the badge back you need to execute command: unblock %AdminID% in Remote Admin.'
  notified_command_text: The administrator used the notified command.
  embed_title: Attention!
  port: Port
  command: Command
  administrator: Administrator
  amount_affected: Amount of affected
  ```
# Pictures
![Picture1](https://user-images.githubusercontent.com/64978711/178718313-c4c9e90b-bc9e-4fbf-9088-81e41b3985cb.png)
![Picture2](https://user-images.githubusercontent.com/64978711/178718321-5340bd22-2395-4039-ba4a-10ea1864fb68.png)
![Picture3](https://user-images.githubusercontent.com/64978711/178718517-afd809e6-d1c7-4243-b26d-bc54b6d9e70d.png)
