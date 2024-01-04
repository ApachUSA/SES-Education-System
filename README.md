# State Emergency Service education system 

## Introduction
The modern world is constantly changing, and new challenges arise in terms of safety and protection of citizens. An important aspect of this safety is the competence of emergency service workers. Annual assessment is important to determine their training, but existing approaches to this process are not effective and difficult to manage and analyze the results. 
Therefore, the aim of this master's thesis is to develop a web-based system for confirming the qualifications of emergency service workers.

## Technologies
![Static Badge](https://img.shields.io/badge/ASP.NET-badge?style=for-the-badge&logo=.net&color=%23292929)
![Static Badge](https://img.shields.io/badge/EF.Core-badge?style=for-the-badge&logo=db&color=%23292929)
![Static Badge](https://img.shields.io/badge/SQL-badge?style=for-the-badge&logo=sql%20server&color=%23292929)
![Static Badge](https://img.shields.io/badge/WPF-badge?style=for-the-badge&logo=WPF&color=%23292929)



<div align="center">
  <img src="/Domain/Screenshots/Architecture.png"/>
  <p>Architecture</p>
  <br/>
</div>

The developed system uses many partial views and ajax for asynchronous requests to the controller.
- The system is built on a three-tier architecture
- It has a web and a desktop version that are responsible for different functions.
- Divided into 3 roles (student, teacher, administrator)

### Web <br/>
Student:
- View personal information and change the password.
- View the results of preparatory and main tests.
- Pass preparatory tests for any specialization.
- View the list of lecture materials.

Teacher:
- View personal information and change the password.
- View the results of the main tests of the students of his department
- Sort results by specialization or date
- Search for results by name
- Import the list of results into a pdf format ready for printing

Administrator:
- Create/edit/delete lecture material
- Create/edit/delete preparatory and main tests
- Create/edit/delete user information

### Desktop <br/>
Student:
- Pass the main test corresponding to his/her specialization. The result is displayed on the site for both the student and his teacher.


