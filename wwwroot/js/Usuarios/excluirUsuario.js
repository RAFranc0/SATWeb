document.getElementById("btnLocalizar").addEventListener("click", function () {
    const select = document.getElementById("selectUsuario");
    const id = select.value;

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
            document.getElementById("formExcluir").style.display = "flex";
            document.getElementById("inputId").value = data.id;
            document.getElementById("campoId").value = data.id;
            document.getElementById("campoNome").value = data.nome;
            document.getElementById("campoDepartamento").value = data.nomeDepartamento;
            document.getElementById("erro").style.display = "none";
        })
        .catch(err => {
            console.error(err);
            document.getElementById("formExcluir").style.display = "none";
            document.getElementById("erro").style.display = "block";
        });
});
