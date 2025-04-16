document.getElementById("btnLocalizar").addEventListener("click", function () {
    const id = document.getElementById("selectUsuario").value;
    if (!id) {
        alert("Selecione um usuário.");
        return;
    }

    fetch(`/Usuarios/ObterUsuarioPorId?id=${id}`)
        .then(res => {
            if (!res.ok) throw new Error("Erro na resposta");
            return res.json();
        })
        .then(data => {
            document.getElementById("formEditar").style.display = "block";
            document.getElementById("inputId").value = data.id;
            document.getElementById("campoNome").value = data.nome;
            document.getElementById("campoDepartamento").value = data.departamentoId;
            document.getElementById("erro").style.display = "none";
        })
        .catch(err => {
            console.error(err);
            document.getElementById("formEditar").style.display = "none";
            document.getElementById("erro").style.display = "block";
        });
});

/* Mostrar formulário se já vier com usuário selecionado
@if (Model.Usuario != null)
{
    <text>
        document.addEventListener("DOMContentLoaded", function() {
        document.getElementById("formEditar").style.display = "block";
        document.getElementById("selectUsuario").value = "@Model.Usuario.Id";
    });
    </text>
}*/