Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace cellaLab_model

Public MustInherit Class db_cellaLab : Inherits IDatabase
Protected ReadOnly m_community As TableModel(Of community)
Protected ReadOnly m_community_formula As TableModel(Of community_formula)
Protected ReadOnly m_culture_medium As TableModel(Of culture_medium)
Protected ReadOnly m_dynamics As TableModel(Of dynamics)
Protected ReadOnly m_experiment As TableModel(Of experiment)
Protected ReadOnly m_model_library As TableModel(Of model_library)
Protected ReadOnly m_model_variant As TableModel(Of model_variant)
Protected ReadOnly m_project_data As TableModel(Of project_data)
Protected ReadOnly m_substrate_composition As TableModel(Of substrate_composition)
Protected ReadOnly m_substrate_modification As TableModel(Of substrate_modification)
Protected ReadOnly m_variants As TableModel(Of variants)
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.m_community = model(Of community)()
Me.m_community_formula = model(Of community_formula)()
Me.m_culture_medium = model(Of culture_medium)()
Me.m_dynamics = model(Of dynamics)()
Me.m_experiment = model(Of experiment)()
Me.m_model_library = model(Of model_library)()
Me.m_model_variant = model(Of model_variant)()
Me.m_project_data = model(Of project_data)()
Me.m_substrate_composition = model(Of substrate_composition)()
Me.m_substrate_modification = model(Of substrate_modification)()
Me.m_variants = model(Of variants)()
End Sub
End Class

End Namespace
