# What?

* AssetImporter を用いる際のヘルパクラスを提供します。

# Why?

* ディレクトリを再帰的に潜って、アセットに処理をするコトが多かったりするので、一部のメソッドを Utility クラスとして切り出しました。

# Install

```shell
$ npm install -D github:umm/assetimporter_helper.git
```

# Usage

```csharp
using UnityModule.EditorUtility;

public class Sample {

    public static void Run() {
        AssetImporterHelper.ActionToSelection<TextureImporter>(
            (importer, assetPath) => {
                // テクスチャに対して何か処理
            }
        );
    }

}
```

# License

Copyright (c) 2017 Tetsuya Mori

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

