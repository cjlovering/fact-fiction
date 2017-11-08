import React from 'react';
import PropTypes from 'prop-types';

export default class InputPane extends React.Component {
	constructor(props) {
	    super(props);
		this.state = {
			textEntry: "Enter some text (news article...)!"
		}
	}

    render() {
        return (
			<div>
				<div className="input-box">
					<textarea 
						rows="10" 
						value={this.state.textEntry}
						onChange={e => this.setState({ textEntry: e.target.value })}
					/>
				</div>
			    <button onClick={() => this.props.addFact(this.state.textEntry)}>
			        Go
			    </button>
			</div>
        );
    }
}

InputPane.propTypes = {
    addFact: PropTypes.func.isRequired
}