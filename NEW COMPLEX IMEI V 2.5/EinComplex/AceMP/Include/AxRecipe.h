#ifndef __AX_RECIPE_H__
#define __AX_RECIPE_H__

#pragma once

#include "AxIni.h"

class __declspec(dllexport) CAxRecipe : public CAxIni 
{
public:
	CAxRecipe();
	virtual ~CAxRecipe();

	void Load();
	void Load(LPCTSTR pszFile);
	void Save();
	void Save(LPCTSTR pszFile);
};

typedef CArray<CAxRecipe, CAxRecipe&>			CAxRecipeArray;
typedef CTypedPtrArray<CPtrArray, CAxRecipe*>	CAxRecipePtrArray;

#endif