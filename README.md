# ArticleManagementApi

This API manages articles of a fictional online store. It supports all CRUD operations for articles and their attributes.

The API is written in .NET 6 and uses EF Core. 

## Use

Run `docker-compose up` in the root directory. It starts the API and a MariaDb server. Now visit `http://localhost/swagger` to see the Swagger documentation.

For development purpose you can use the launch configurations inside Visual Studio or Jetbrains Rider. For the MariaDb server run `docker-compose up` in the mariadb/ directory.
You can reach Swagger at 'https://localhost:5001/swagger'.

I created a Docker run configuration inside Jetbrains Rider but sadly it's not added to the launch configurations file. Later I might add it via Visual Studio.

## Requirements

* articles are identified by their article number
* properties for articles:
  * title
  * description
  * color
  * is the article bulky?
  * brand
* properties of article are stored for Germany, France, Italy and Switzerland
* articles can be searched 
  * by the date they were changed the last time
  * title
* articles are approved if they have all needed properties

## Implementation

Some properties are of course the same for all countries. These are stored inside the article model. 
Properties that can be different for each country are modeled as attributes of articles.

Articles consists of 
* article number
* brand
* flag if article is approved
* flag if article is bulky
* date of last change
* list of attributes for different countries

Attributes of articles constists of 
* title
* description
* color
* country
* date of last change

Attributes are identified by the article number of their article and their country.

One article can't have multiple attributes for the same country.

To keep the response small and clearly structured the attributes of a article are not sent if a article is requested. This can be useful e.g. if more countries are added. 
Also users of the api might only want to get the basic properties of the article so this approach reduces unecessary network load.

## Tests

The API is currently not tested. I might add unit tests later.
