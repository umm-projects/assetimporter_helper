using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityModule.EditorUtility {

    /// <summary>
    /// AssetImporter 用のヘルパ
    /// </summary>
    public static class AssetImporterHelper {

        /// <summary>
        /// 選択しているアセットを操作する
        /// </summary>
        /// <param name="action">選択アセットに対して実行する処理</param>
        public static void ActionToSelection<T>(Action<T, string> action) where T : AssetImporter {
            Object[] selectedAssets = Selection.objects;
            if (selectedAssets == default(Object[]) || selectedAssets.Length == 0) {
                Debug.LogWarning("1つ以上のアセットを選択してください。");
                return;
            }
            IEnumerable<Object> normalizedSelectedAssetList = FlattenSelectedAssets();
            foreach (Object asset in normalizedSelectedAssetList) {
                string assetPath = AssetDatabase.GetAssetPath(asset);
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer is T) {
                    action(importer as T, assetPath);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 選択されているアセットをフラット化したリストを返す
        /// </summary>
        /// <remarks>ディレクトリを選択している場合は配下のアセットにバラして返す</remarks>
        /// <returns>フラット化したアセットのリスト</returns>
        private static IEnumerable<Object> FlattenSelectedAssets() {
            List<Object> assetList = new List<Object>();
            foreach (Object selectedAsset in Selection.objects) {
                string path = AssetDatabase.GetAssetPath(selectedAsset);
                if (AssetDatabase.IsValidFolder(path)) {
                    string[] guids = AssetDatabase.FindAssets(
                        "t:Object",
                        new [] {
                            path,
                        }
                    );
                    foreach (string guid in guids) {
                        // ディレクトリには AssetBundle 名を付与しないためスキップ
                        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                        if (AssetDatabase.IsValidFolder(assetPath)) {
                            continue;
                        }
                        Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                        assetList.Add(asset);
                    }
                } else {
                    assetList.Add(selectedAsset);
                }
            }
            return assetList;
        }


    }

}
