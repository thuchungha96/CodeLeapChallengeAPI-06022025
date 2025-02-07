Tech use:
<br/>
![ChatGPT](https://img.shields.io/badge/chatGPT-74aa9c?style=for-the-badge&logo=openai&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)
![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
<br/>
Hosting:
<br/>
![Render](https://img.shields.io/badge/Render-%46E3B7.svg?style=for-the-badge&logo=render&logoColor=white)
<br/>
Swagger: https://codeleapchallengeapi-06022025.onrender.com <br/>
sample Setup for another:
1.Register new account from https://render.com/
![image](https://github.com/user-attachments/assets/ec019cfb-6dc2-4e76-848a-88901fd663c6)

2.Create new PostgreSQL and get connectstring
![image](https://github.com/user-attachments/assets/4e7a28b4-e852-49fb-bf28-7d5f8aeab606)

3.Copy postgres://root:hd72s7wRDeEnOuGTNtiuWi4If1LtY1yI@dpg-cp71886v3ddc73fs8qqg-a.oregon-postgres.render.com/db_bsdb Example: connect string C# format postgres//{User Id}:{Password}@{Server}/{Database}
Open appsettings.json in source code, find "CodeDB", and change it follow prev step
![image](https://github.com/user-attachments/assets/2372d482-9f62-4f84-a133-497a466df1c9)

4.Change JWT Secret key for more security follow prev step.

5.Visual Studio -> Tools -> Nuget package manager -> Pakage manager console. Add-Migration InitialCreate
![image](https://github.com/user-attachments/assets/cfa29448-30a2-482e-bc07-24922cff2524)

6.Visual Studio -> Tools -> Nuget package manager -> Pakage manager console. Update-Database
![image](https://github.com/user-attachments/assets/805261d7-2db5-4c93-b914-35bc25afc054)

=> Done setup postgreSQL

7.Create new Webservice from render.com
![image](https://github.com/user-attachments/assets/9086d48d-4636-4be2-8e83-812a26b8eee6)
![image](https://github.com/user-attachments/assets/af9d62d4-1103-4a38-b599-6bf272eac72c)
![image](https://github.com/user-attachments/assets/9304773b-7ea0-470c-bae5-7b18a421e0e1)

8.Select current project from github
![image](https://github.com/user-attachments/assets/5c74a771-846b-46b9-a81d-eb4335d6ea27)
![image](https://github.com/user-attachments/assets/d14b2fba-7737-4e86-87e8-27046f4631bb)

9.After that, get deploy hook for auto build CI/CD deploy new image into render server.
![image](https://github.com/user-attachments/assets/427cff4b-f809-49d6-85e6-d4995fd9dc4a)

10.Create new yaml file follow photo(select master branch or any branch).
![image](https://github.com/user-attachments/assets/168b979e-df62-4d08-8109-62583d9f0beb)

11.Go to setting-> secrets and variables, add new key RENDER_DEPLOY_HOOK_API
![image](https://github.com/user-attachments/assets/42d044f1-0665-44df-8890-5de84efdf8e0)
-> Finish setup.



