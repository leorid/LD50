using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JL
{
	public class CodeGen : EditorWindow
	{
		public CodeGenSaveData saveData;
		public VisualTreeAsset windowPrefab;
		public VisualTreeAsset replacerPrefab;

		WindowArea windowArea;

		CodeGenTemplate currentTemplate;

		public int lastOne;

		[MenuItem("Tools/AccessML")]
		static void AccessML()
		{
			Debug.Log("ML.Enemy.detection: " + ML.Enemy.detection.ID);
		}

		[MenuItem("Tools/Recompile")]
		static void Recompile()
		{
			UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
		}


		[MenuItem("Tools/CodeGen")]
		static void Init()
		{
			var window = EditorWindow.GetWindow<CodeGen>();
			window.titleContent = new GUIContent("CodeGen");
			window.Show();
		}

		private void CreateGUI()
		{
			VisualElement root = rootVisualElement;
			root.Clear();

			if (!saveData) { Debug.LogError("No SaveData found"); return; }

			if (saveData.templates.Count == 0)
			{
				CodeGenTemplate template = new CodeGenTemplate();
				saveData.templates.Add(template);
			}

			TemplateContainer windowInstance = windowPrefab.Instantiate();
			root.Add(windowInstance);
			windowArea = new WindowArea(windowInstance);

			BuildSelectionArea();

			lastOne = Mathf.Clamp(lastOne, 0, saveData.templates.Count - 1);

			BuildFor(saveData.templates[lastOne]);
		}

		void BuildSelectionArea()
		{
			windowArea.updateButton.clickable.clicked -= CreateGUI;
			windowArea.updateButton.clickable.clicked += CreateGUI;

			windowArea.createNew.clickable.clicked -= CreateNewTemplate;
			windowArea.createNew.clickable.clicked += CreateNewTemplate;

			List<string> choices = new();
			foreach (CodeGenTemplate template in saveData.templates)
			{
				if (string.IsNullOrWhiteSpace(template.name))
				{
					template.name = "Unnamed";
				}
				choices.Add(template.name);
			}
			windowArea.chooseExisting.choices = choices;
			windowArea.chooseExisting.SetValueWithoutNotify(choices[lastOne]);

			windowArea.chooseExisting.UnregisterValueChangedCallback(ChoseExisting);
			windowArea.chooseExisting.RegisterValueChangedCallback(ChoseExisting);

			windowArea.nameField.UnregisterValueChangedCallback(ChangeName);
			windowArea.nameField.RegisterValueChangedCallback(ChangeName);

			windowArea.input.UnregisterValueChangedCallback(InputChanged);
			windowArea.input.RegisterValueChangedCallback(InputChanged);

			windowArea.buildButton.clickable.clicked -= Build;
			windowArea.buildButton.clickable.clicked += Build;

			windowArea.addReplacerButton.clickable.clicked -= AddReplacer;
			windowArea.addReplacerButton.clickable.clicked += AddReplacer;
		}

		void ChoseExisting(ChangeEvent<string> e)
		{
			string choice = e.newValue;
			for (int i = 0; i < saveData.templates.Count; i++)
			{
				CodeGenTemplate template = saveData.templates[i];

				if (template.name == choice)
				{
					lastOne = i;
					BuildFor(template);
				}
			}
		}
		void CreateNewTemplate()
		{
			CodeGenTemplate template = new CodeGenTemplate();
			saveData.templates.Add(template);
			BuildSelectionArea();
			BuildFor(template);
			EditorUtility.SetDirty(saveData);
		}
		void AddReplacer()
		{
			currentTemplate.replacers.Add(new CodeGenReplacer());
			EditorUtility.SetDirty(saveData);
			BuildFor(currentTemplate);
		}
		void Build()
		{
			string result = "";
			int max = 0;
			foreach (CodeGenReplacer replacer in currentTemplate.replacers)
			{
				max = Mathf.Max(max, replacer.replacements.Count);
			}
			for (int i = 0; i < max; i++)
			{
				string tempResult = currentTemplate.templateString;
				foreach (CodeGenReplacer replacer in currentTemplate.replacers)
				{
					tempResult = tempResult.Replace(replacer.existing,
						replacer.replacements[i]);
				}

				result += tempResult;
				result += "\r\n";
			}
			windowArea.output.SetValueWithoutNotify(result);
		}

		void BuildFor(CodeGenTemplate template)
		{
			currentTemplate = template;

			windowArea.nameField.SetValueWithoutNotify(currentTemplate.name);
			windowArea.input.SetValueWithoutNotify(currentTemplate.templateString);
			windowArea.output.SetValueWithoutNotify("");

			windowArea.replacerArea.Clear();
			foreach (CodeGenReplacer replacer in currentTemplate.replacers)
			{

				TemplateContainer replacerInstance = replacerPrefab.Instantiate();
				windowArea.replacerArea.Add(replacerInstance);
				Replacement replacement = new Replacement(replacerInstance);

				List<string> replacements = replacer.replacements;

				// temp for lambda
				CodeGenReplacer temp = replacer;
				replacement.deleteButton.clickable.clicked += () =>
				{
					currentTemplate.replacers.Remove(temp);
					EditorUtility.SetDirty(saveData);
					BuildFor(currentTemplate);
				};

				replacement.nameField.SetValueWithoutNotify(replacer.name);
				replacement.nameField.RegisterValueChangedCallback(e =>
				{
					replacer.name = e.newValue;
					EditorUtility.SetDirty(saveData);
				});

				replacement.existing.SetValueWithoutNotify(replacer.existing);
				replacement.existing.RegisterValueChangedCallback(e =>
				{
					replacer.existing = e.newValue;
					EditorUtility.SetDirty(saveData);
				});

				replacement.listArea.itemsSource = replacements;
				replacement.listArea.makeItem = () =>
				{
					VisualElement parent = new VisualElement();
					parent.Add(new TextField());
					return parent;
				};
				replacement.listArea.bindItem = (v, i) =>
				{
					v.Clear();
					TextField field = new TextField();
					field.SetValueWithoutNotify(replacements[i]);
					v.Add(field);
					field.RegisterValueChangedCallback(e =>
					{
						replacements[i] = e.newValue;
					});
				};
			}
		}

		void InputChanged(ChangeEvent<string> e)
		{
			currentTemplate.templateString = e.newValue;
			EditorUtility.SetDirty(saveData);
		}
		void ChangeName(ChangeEvent<string> e)
		{
			BuildSelectionArea();
			currentTemplate.name = e.newValue;
			EditorUtility.SetDirty(saveData);
		}
	}

	class WindowArea
	{
		public VisualElement root;

		public TextField nameField, input, output;
		public Button updateButton, createNew, buildButton, addReplacerButton;
		public DropdownField chooseExisting;
		public VisualElement replacerArea;

		public WindowArea(VisualElement root)
		{
			this.root = root;
			updateButton = root.Q<Button>("UpdateButton");
			nameField = root.Q<TextField>("NameField");
			buildButton = root.Q<Button>("BuildButton");
			input = root.Q<TextField>("Input");
			output = root.Q<TextField>("Output");
			createNew = root.Q<Button>("CreateNewButton");
			chooseExisting = root.Q<DropdownField>("ChooseExistingDropdown");
			replacerArea = root.Q<VisualElement>("ReplacerArea");
			addReplacerButton = root.Q<Button>("AddButton");
		}
	}
	class Replacement
	{
		public VisualElement root;

		public Button deleteButton;
		public TextField nameField, existing;
		public ListView listArea;

		public Replacement(VisualElement root)
		{
			this.root = root;
			nameField = root.Q<TextField>("ReplacerName");
			existing = root.Q<TextField>("ReplacerExisting");
			listArea = root.Q<ListView>("ListView");
			deleteButton = root.Q<Button>("DeleteButton");
		}
	}



	[System.Serializable]
	public class CodeGenTemplate
	{
		public string name = "New";
		public string templateString;

		public List<CodeGenReplacer> replacers;

		public CodeGenTemplate()
		{
			replacers = new List<CodeGenReplacer>()
			{
				new CodeGenReplacer()
				{
					name = "<T1>",
					existing = "<T1>",
					replacements = new List<string>()
					{
						"<T1>",
						"<T1, T2>",
						"<T1, T2, T3>",
					}
				}
			};
		}
	}

	[System.Serializable]
	public class CodeGenReplacer
	{
		public string name = "New";
		public string existing;
		public List<string> replacements = new();
	}
}