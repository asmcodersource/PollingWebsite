import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import './CreateNewItemDialog.css'
import { useState } from 'react';

const availableDiscriminators : String[] = [
    "TextFieldQuestion",
    "SelectQuestion",
]

interface CreateQuestionDTO {
    questionName: string,
    questionDescription: string,
    questionDiscriminator: string
}

const CreateNewItemDialog = (props: any) => {
    const [errorMsg, setErrorMsg] = useState<string>();

    function createItem() {
        const questionName = document.querySelector('.create-new-item-dialog #field-name')?.value;
        const questionDescription = document.querySelector('.create-new-item-dialog #field-description')?.value;
        const questionDiscriminator = document.querySelector('.create-new-item-dialog #field-discriminator')?.value;

        let createQuestionDTO: CreateQuestionDTO = {
            questionName: questionName,
            questionDescription: questionDescription,
            questionDiscriminator: questionDiscriminator 
        };

        const questionsOrderRateUpdate = new Promise(async (resolve, reject) => {
            let response: Response | null = null;
            try {
                response = await fetch(`/api/questions/${props.quiz.id}`, {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json",
                        "Authorization": "Bearer " + sessionStorage.getItem('token')
                    },
                    body: JSON.stringify(
                        { ...createQuestionDTO }
                    )
                });
                if (response.status == 200)
                    resolve(response);
                else
                    reject(response);
            } catch {
                reject(response);
            }
        });
        questionsOrderRateUpdate.then(
            async (response: Response) => {
                props.onUpdate();
                props.onHide();
            },
            async (response: Response) => {
                let responseBody = await response.json();
                console.log(responseBody);
                if (responseBody.hasOwnProperty('errors')) {
                    let errorMessage = ''
                    for (const error in responseBody['errors']) {
                        for (const msg of responseBody['errors'][error]) {
                            errorMessage = errorMessage + msg + ". ";
                        }
                    }
                    setErrorMsg(errorMessage);
                } else {
                    setErrorMsg("An unknown error has occured");
                }
            }
        )
    }

    return (
        <Modal className="create-new-item-dialog"
            fullscreen={true}
            show={props.show}
            onHide={props.onHide}
            dialogClassName="modal-90w"
            aria-labelledby="example-custom-modal-styling-title"
        >
            <Modal.Header closeButton>
                <Modal.Title id="example-custom-modal-styling-title">
                    Create new item
                </Modal.Title>
            </Modal.Header>
            <Modal.Body className="body">
                <div className="limiter-container-wrapper">
                    <div className="limiter-container">
                        <label>Field name</label><br />
                        <input id="field-name" placeholder="name of question property"></input><br />
                        <label>Field description</label><br />
                        <input id="field-description" placeholder="visible description of question"></input><br />
                        <label>Object type</label><br />
                        <select id="field-discriminator">
                            {availableDiscriminators.map((option, index) => <option key={index}>{option}</option>)}
                        </select>
                        <br />
                        <p className="text-secondary text-justify">The field name field is needed for internal data processing needs (in the future it can be used to export data from the site). The description field is the message that will be displayed before a specific input/selection field. The object type determines what type of question will be created. After creation, you can edit the created question.</p>
                        <p className="error-box">
                            {errorMsg}
                        </p>
                        <div className="buttons-wrapper">
                            <Button onClick={createItem} className="create" variant="primary">Create</Button>
                        </div>
                    </div>
                </div>
            </Modal.Body>
        </Modal>
    )
}

export default CreateNewItemDialog;