# MarketsShark

This project is a web application that compares various hardware components, like computers, laptops etc. from different popular websites based on the criteria inserted by the user in the graphical user interface. The project achieves this by web-scrapping the popular vendors’ websites and retrieving the collected information in our website’s graphical user interface. The project will not contain a physical storage for the searched products, as it only retrieves information from vendors’ websites and display it for the user, eventually allowing the user to buy directly from our website. This way, our website acts as an intermediate between the user (the buyer) and the vendors. 
The project will be split into two parts: the server side (the API) and the client side. For the first part, C# will be used, while for the client side Angular will be used, which implies HTML, CSS and Typescript. 

            
The application is a website that collects information from various well-known hardware components vendors. In order to achieve this, the priciple of web-scrapping is used: based on the filtering criteria taken from the user input, searching links will be generated in the format of each vendor’s website. Then, using Regex pattern matching, the data will be collected from the HTML content of the page and objects containing the appropriate information will be created in memory, which will then be displayed graphically in the user interface. The user can then go to the vendor’s website to see the product more detailed or even buy it from there, or buy it straight from our website. The payment is registered in our account, which will then be redirected to the original vendor. 
For faster data loading and more responsiveness, we used Angular’s modular development strategy, which enables lazy-loading, i.e. loading each module only when it is needed, instead of loading them all at the beginning of the session. 


The project is adapted after Neil Cumming's Udemy tutorial "Learn to build an e-commerce app with .Net Core and Angular", at https://www.udemy.com/course/learn-to-build-an-e-commerce-app-with-net-core-and-angular/.


Project made with C# (.NET Core 3.1) - for the server side, and Angular 9 - for the client side respectively. 
