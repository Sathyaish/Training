#pragma once

#include <stdio.h>
#include <map>

#define Reflect 

void ReflectRegisteredFunctions();

class Reflection
{
	private:
		static Reflection _singleton;
		std::map<const char*, void (*)()> _nameToFunc;

	public:
		static void Insert(const char* name, void (func)())
		{
			_singleton._nameToFunc[name] = func;
		}

		static void PrintAll()
		{
			auto iter = _singleton._nameToFunc.begin();
			while(iter != _singleton._nameToFunc.end())
			{
				printf("%s\n", iter->first);
				++iter;
			}
		}

		static void CallFunction(const char* name)
		{
			_singleton._nameToFunc[name]();
		}
};

void func1();
void func2();
void func3();
void func4();
void func5();
void func6();
void func7();
void func8();