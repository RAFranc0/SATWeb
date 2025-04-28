document.getElementById("btnLocalizar").addEventListener("click", function () {
    const select = document.getElementById("selectDepartamento");
    const id = select.value;

    if (!id) {
        alert("Selecione um departamento.");
        return;
    }

    fetch(`/Departamentos/ObterDepartamento?id=${id}`)
        .then(res => {
            if (!res.ok) throw new Error("Erro na resposta");
            return res.json();
        })
        .then(data => {
            document.getElementById("formExcluir").style.display = "flex";
            document.getElementById("inputId").value = data.id;
            document.getElementById("campoId").value = data.id;
            document.getElementById("campoNome").value = data.nome;
            document.getElementById("erro").style.display = "none";
        })
        .catch(err => {
            console.error(err);
            document.getElementById("formExcluir").style.display = "none";
            document.getElementById("erro").style.display = "block";
        });
});