# [FolderDoc](http://diev.github.io/FolderDoc/)

[![Build status](https://ci.appveyor.com/api/projects/status/naqbx5c8kbjw2hmp?svg=true)](https://ci.appveyor.com/project/diev/folderdoc)
[![GitHub Release](https://img.shields.io/github/release/diev/FolderDoc.svg)](https://github.com/diev/FolderDoc/releases/latest)

Folders and Documents linkable in both directions.  
Папки и документы со ссылками в обе стороны.

## Сценарии использования

### Сценарий 1: Папка с файлами

Эмуляция обычной файловой папки с документами и подпапками с документами и в 
них - типичная иерархическая структура. У каждого документа/подпапки `Item` 
есть один `Parent` (нет его только у корневой папки) и сколько-то возможных 
`Children`. Собственно, такую иерархию можно было организовать и одной *Self 
Referencing* таблицей `Items`, но в такую модель данных заложена б*о*льшая 
универсальность, как это показано в нижеследующих сценариях.

Именно по этому сценарию загружается начальный образец данных по древовидному
содержимому папки `MyDocuments` из профиля текущего пользователя.

### Сценарий 2: Тегированные папки с пересекающимися файлами

К ситуации сценария 1 добавляется возможность иметь более одного `Parent`, 
что позволяет на один и тот же документ/подпапку `Item` попадать из разных 
мест по навигационному свойству `Children` и также перемещаться **в обратные 
стороны** по свойству `Parents`. Эту возможность дает таблица *Many-to-Many* 
`Links` с коллекцией пар связанных ею ключей `ParentId` и `ChildId`. 

Такой взаимосвязанный документ становится "общим" для нескольких папок - без 
необходимости его дублировать в каждую из них (как при использовании сценария
1) или создания линков средствами файловой системы. Более того, приложения к 
документу теперь выглядят действительно как приложения `Children` к `Item` - 
без необходимости создания дополнительных подпапок или загромождения 
файловой системы рядом лежащими файлами без иерархической структуры:

 * документ - приложения/фотографии к нему;
 * исходный текстовый документ - сканы страниц подписанного документа;
 * документ - разные редакции для разных назначений.

Таким же образом можно организовать версионность документов - все в единой 
системе иерархичности - без необходимости подпапок и разрастания дублей 
одних файлов в одной куче с дублями других.

Также это позволяет создать систему, где каждый пользователь может хранить 
свои собственные коллекции документов и папок в таблице `Folders` - 
независимо от физической организации хранения файлов на диске или в базе. 
Сами файлы при этом можно переименовать по GUID `Item.Id` - для единообразия 
и большей сохранности - в том числе и в целях информационной безопасности, 
ограничив к ним доступ как на извлечение отдельных документов, так и на 
хищение всей папки с документами.

И главное - можно выполнить *полнотекстовый поиск* для получения определенного 
документа, а от него уже найти все родительские папки/документы, с которыми он 
взаимосвязан.

*Возможно, все это уже сделано в Microsoft SharePoint, но с каждой новой версией 
он все монструознее и с все большими требованиями к ресурсам, в том числе 
лицензионным, а это решение нацелено на собственные задачи и возможность 
миграции в среду ASP.NET Core на серверах без Windows.*

### Сценарий 3: Каталог любых элементов

Предыдущий сценарий 2 расширяется до универсального хранилища вообще любых 
элементов задаваемого класса `Item`, который может входить сразу в несколько 
категорий `Parents` товаров, например, и при этом также иметь несколько 
сопутствующих товаров `Children`.

Это можно еще представить как интернет-магазин запчастей, где они подходят 
сразу к нескольким моделям (узлам) изделий и сами они, в свою очередь, 
содержат более мелкие детали. Вся информация от этом управляется классом 
`Link` (таблица `Links`), а описание каждого элемента определяется классом 
`Item` (таблица `Items`). Сами каталоги деталей - класс `Folder` (таблица 
`Folders`). В дополнение - каждый элемент может содержать ссылки на файлы 
с инструкциями, лежащими на диске.

## Особенности реализации

Проект создан на C# с применением Entity Framework с подходом *Code First* 
с использованием WinForms и NET Framework 4 для работы в Windows XP и выше.

## License

Licensed under the [Apache License, Version 2.0](LICENSE).
