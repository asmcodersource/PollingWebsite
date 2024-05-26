import './QuizGeneralSettings.css';
import Button from 'react-bootstrap/Button';

const options: String[] = [
    "Anyone",
    "Auhorized",
    "Only allowed",
    "Only owner"
]

const QuizGeneralSettings = (props) => {
    console.log(props);
    return (
        <div className="row quiz-manager-general-wrapper">
            <label>Title</label>
            <input
                type="text"
                placeholder="Visible title of the survey"
                value={props.quiz.title}
            >
            </input><br />
            <label>Description</label>
            <textarea
                className="grow"
                rows="10"
                value={ props.quiz.description  }
                placeholder="Enter a survey description here, which will be visible to all users who land on the survey page.">
            </textarea><br />
            <label>Access type</label>
            <select>
                {options.map((option, index) => <option key={index}>{option}</option>)}
            </select>
            <div className="buttons-wrapper">
                <Button variant="primary">Save changes</Button>
            </div>
        </div>
    )
}


export default QuizGeneralSettings;