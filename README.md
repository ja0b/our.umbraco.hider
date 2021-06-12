
# Umbraco Hider

[![Build Status](https://travis-ci.com/ja0b/our.umbraco.hider.svg?branch=master)](https://travis-ci.com/ja0b/our.umbraco.hider) [![NuGet release](https://img.shields.io/nuget/v/Our.Umbraco.Hider.svg)](https://www.nuget.org/packages/Our.Umbraco.Hider) [![NuGet](https://img.shields.io/nuget/dt/Our.Umbraco.Hider.svg)](https://www.nuget.org/packages/Our.Umbraco.Hider) [![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.org/projects/backoffice-extensions/cogworks-examine-inspector) [![Umbraco version](https://img.shields.io/badge/umbraco->8.10.0-%233544b1)](https://github.com/ja0b/Our.Umbraco.Hider)

This package makes it possible to create rules in the umbraco backoffice to hide (tabs, properties, buttons <<Including the actions button>> and content apps) for certain Users, User Groups, Content Ids, Parent Content Ids and/or Content Types.

## Getting Started 💫

This package is supported on Umbraco 8.10.0+.
(It may work for lower 8 versions but it has not been tested)

## Installation 🎊

UmbracoHider is available from NuGet.

#### NuGet package repository
To [install from NuGet](https://www.nuget.org/packages/Our.Umbraco.Hider/), you can run the following command from within Visual Studio:

	PM> Install-Package Our.Umbraco.Hider

## Usage 🔥

After installing the package you will see new tab inside the setting dashboard to create your rules.

Dashboard
![UmbracoHider Dashboard](docs/img/UmbracoHider_1.jpg?raw=true)

Rule Types
![UmbracoHider Rule Types](docs/img/UmbracoHider_2.jpg?raw=true)

## Contribution guidelines ⛏

To raise a new bug, create an issue on the GitHub repository. To fix a bug or add new features, fork the repository and send a pull request with your changes. Feel free to add ideas to the repository's issues list if you would to discuss anything related to the package.

## Notes 📝
This package was built using https://our.umbraco.com/packages/collaboration/backoffice-tweaking/ and https://github.com/janvanhelvoort/Umbraco-hide-properties as inspiration, so big kudos to Alain and Jan for their work.

Finally thanks to [Imran Haider](https://twitter.com/Jim_Randy) that without knowing gave his last name to name this package 😂.

![UmbracoHider Name](docs/img/UmbracoHider_3.jpg?raw=true)

## License 📜
Licensed under the [MIT License](LICENSE)