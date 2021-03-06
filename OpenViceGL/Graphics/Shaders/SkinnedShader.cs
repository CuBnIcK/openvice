﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenVice.Graphics.Shaders {

	/// <summary>
	/// Shader for skinned meshes<para/>
	/// Шейдер для заскинненых мешей
	/// </summary>
	public class SkinnedShader : ShaderBase {

		/// <summary>
		/// Internal shader object<para/>
		/// Внутренний объект шейдера
		/// </summary>
		static SkinnedShader shader;

		/// <summary>
		/// Shader object access field<para/>
		/// Поле доступа к объекту шейдера
		/// </summary>
		public static SkinnedShader Shader {
			get {
				if (shader == null) {
					shader = new SkinnedShader();
					shader.CompileShader();
				}
				return shader;
			}
		}

		/// <summary>
		/// Seek associated uniforms<para/>
		/// Поиск ассоциированных униформов
		/// </summary>
		protected override void SeekUniforms() {
			GL.UseProgram(glprog);
			ProjectionMatrix = GL.GetUniformLocation(glprog, "projMatrix");
			ModelViewMatrix = GL.GetUniformLocation(glprog, "modelMatrix");
			ObjectMatrix = GL.GetUniformLocation(glprog, "objectMatrix");
			//TopColor = GL.GetUniformLocation(glprog, "topColor");
			//BottomColor = GL.GetUniformLocation(glprog, "bottomColor");
			GL.Uniform1(GL.GetUniformLocation(glprog, "texture"), 0);
			GL.UseProgram(0);
		}

		/// <summary>
		/// Get fragment shader code<para/>
		/// Получение кода фрагментного шейдера
		/// </summary>
		protected override string GetFragmentCode() {
			return fragmentProg;
		}

		/// <summary>
		/// Get vertex shader code<para/>
		/// Получение кода вершинного шейдера
		/// </summary>
		protected override string GetVertexCode() {
			return vertexProg;
		}

		/// <summary>
		/// Camera projection matrix<para/>
		/// Матрица проекции камеры
		/// </summary>
		public static int ProjectionMatrix { get; private set; }

		/// <summary>
		/// Camera position matrix<para/>
		/// Матрица расположения камеры
		/// </summary>
		public static int ModelViewMatrix { get; private set; }

		/// <summary>
		/// Object matrix<para/>
		/// Матрица объекта
		/// </summary>
		public static int ObjectMatrix { get; private set; }

		/// <summary>
		/// Top color<para/>
		/// Цвет верхней части
		/// </summary>
		public static int TopColor { get; set; }

		/// <summary>
		/// Bottom color<para/>
		/// Цвет нижней части
		/// </summary>
		public static int BottomColor { get; set; }

		/// <summary>
		/// Vertex program for this shader<para/>
		/// Вершинная программа для этого шейдера
		/// </summary>
		static string vertexProg = @"
			// Basic uniforms list
			// Список юниформов
			uniform mat4 projMatrix;
			uniform mat4 modelMatrix;
			uniform mat4 objectMatrix;
			uniform mat4 bones[64];
			
			// Texture coords
			// Текстурные координаты
			varying vec2 texCoords;
			
			// Processing vertex
			// Обработка вершины
			void main() {
				texCoords = gl_MultiTexCoord0.xy;
				mat4 completeMat = projMatrix * modelMatrix * objectMatrix;
				
				gl_Position = completeMat * gl_Vertex;
			}
		";

		/// <summary>
		/// Fragment program for this shader<para/>
		/// Фрагментная программа для этого шейдера
		/// </summary>
		static string fragmentProg = @"
			// Basic uniforms list
			// Список юниформов
			uniform sampler2D texture;
			
			// Texture coords
			// Текстурные координаты
			varying vec2 texCoords;

			// Processing fragment
			// Обработка фрагмента
			void main() {
				gl_FragColor = vec4(texture2D(texture, texCoords.xy).rgb, 1.0);
			}
		";

	}
}
