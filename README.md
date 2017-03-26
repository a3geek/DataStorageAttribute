DataStorageAttribute
====


## Description
クラスのフィールドに適用することで、フィールドの値を自動で保存・反映するAttribute(属性)です。  

アプリケーション終了時に値を保存し、次回起動時に以前の値を反映させたい時、PlayerPrefs.〇〇を大量に記述する必要や保存用のクラスを一つ用意してそこに値を集約させる手間が必要になります。  
その手間を無くし、自動で上記の様な挙動を実現するのが目的です。  

Unityでの利用を前提としています。  
プロパティには対応していません。  

## Usage
1. Attributes/DataStorage/Prefabs/Data Storage Controller.prefabをゲーム空間に置いてください。
2. 値の保存・読込を行いたいフィールドにDataStorageAttributeを適用してください。
    - Attributeが適用されたフィールドのクラスのインスタンスを探すのにFindObjectsOfType<>メソッドを利用しています。
    - なので、Attributeを適用するフィールドのクラスは必ずMonoBehaviourを継承している必要があります。
3. Data Storage ControllerコンポーネントのInspectorでInitialize Dataボタンを押してください。
    - DataStorageController.csのInitializeメソッドが実行されます。
    - 現在のゲーム空間内に存在する全てのゲームオブジェクトにアタッチされている全てのコンポーネントについて、Attributeが適用されたフィールドがないか検索を行います。

``` csharp
using a3geek.Attributes;

public class Test : MonoBehaviour
{
    // 保存KeyとしてIntValueが使われる.
    [DataStorage("IntValue")]
    public int value1 = 1;

    // 自動で保存Keyを生成する.
    [DataStorage]
    private float value2 = 10f;
}
``` 

## Behaviour
- 変数名の変更や新規でフィールド追加等を行ったら、Initializeメソッドを実行してください。
    - 自動で変更検知を行わないので、エラーになったり新規追加分について動作しなかったりする事があります。
    - アプリ実行の度にInitializeメソッドを実行する形にしても問題はないですが、重たい処理が走る事になるのでリリース版のビルド前にはEditor上で実行しておくのがオススメです。
    - Editor上でInitialize Dataを行うと、Unityのメタデータに依存してデータの保持を行っているので、アプリ実行時にInitializeメソッドを実行しなくても動作します。
- 保存用のKey情報を明示する事が出来ます。
    - Attributeを適用する時にKeyを渡すと、渡されたKeyで保存します。
    - Keyが渡されなかった場合は、自動で保存用のKeyを生成します。
        - #{CompanyName}:#{ProjectName}:#{SceneName}->#{ParentsName}/#{GameObjectName}->#{NameSpace}.#{ClassName}.#{FieldName}が保存Keyになります。
- 保存と読込の処理をカスタムする事が出来ます。
    - DataStorageControllerコンポーネントのInspectorでSaverとLoaderのUnityEventにそれぞれメソッドを適用しておく事で、適用されたメソッドで保存と読込を行います。
    - デフォルトでは[XmlStorage](https://github.com/a3geek/XmlStorage)を利用して保存・読込を行います。
- Attribtueの検索処理は以下の流れで行っています。
    - Assembly.GetExecutingAssemblyメソッドでユーザ定義のクラスを取得します。
    - MonoBehaviourを継承しているクラスだけを抜き出します。
    - Attributeが適用されたフィールドが存在するクラスだけを抜き出します。
    - クラス型でFindObjectsOfTypeを行いインスタンスを検索します。
    - コンポーネントへの参照とフィールド名、保存Key情報をDataStorageControllerで保持を行い、Unityのメタデータに保存します。

## API
### `DataStorageController`クラス
保存・読込の挙動の制御と、Attributeの検索・管理を行います。  
シングルトンを実装してありますので、`DataStorageController.Instance`でアクセス出来ます。

### プロパティ
#### `bool LoadOnAwake { get; }`
Awakeの時にLoadメソッドを実行するかどうか

#### `bool SaveOnQuit { get; }`
OnApplicationQuitの時にSaveメソッドを実行するかどうか

#### `bool InitializeOnAwakeInRuntime`
アプリ実行時のAwakeの時にInitializeメソッドを実行するかどうか

#### `bool InitializeOnAwakeInEditor`
Editor実行時のAwake時にInitializeメソッドを実行するかどうか

### プロパティ(Inspector only)
#### `Storage`
保存しているコンポーネントとフィールド情報

#### `Saver`
保存処理のカスタム用

#### `Loader`
読込処理のカスタム用

### メソッド
#### `void Save()`
保存処理を行う

#### `void Load()`
読込処理を行う

#### `void Initialize()`
コンポーネントとフィールド情報の検索する
